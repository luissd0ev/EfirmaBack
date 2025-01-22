using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data;
using Dapper;
using Org.BouncyCastle.Crypto;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Ocsp;
using System.Net;
using System.Collections;
using QRCoder;
using System.Drawing;
using System.Net.Http;
using Abp.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using APIEfirma.Models; 

namespace APIEfirma.Utility
{
   public static class Tools
   {
      public static string GenerateTokeUser(int idUser)
      {
         return $"{idUser}-{DateTime.Now.ToString("MMddHHmmssffff")}";
      }
      public static string GenerateTokeUser(string nameuser)
      {
         return $"{new Random().Next(1, 1000)}-{DateTime.Now.ToString("MMddHHmmssffff")}";
      }
      public static DataCurp GetDataToCurp(string curp)
      {
         try
         {
            DataCurp result = new DataCurp();
            using (var client = new HttpClient())
            {
               HttpResponseMessage response = AsyncHelper.RunSync<HttpResponseMessage>(() => client.GetAsync("https://sistemas.sedatu.gob.mx/renapo/curp/" + curp)); ;
               if (response.IsSuccessStatusCode)
               {
                  string resultstream = AsyncHelper.RunSync<string>(() => response.Content.ReadAsStringAsync());
                  List<string> temp = resultstream.Split(',').ToList();
                  if (temp.Exists(x => x.Contains("nombres")))
                     result = new DataCurp(temp.Find(x => x.Contains("nombres")).Split(':')[1].Trim('"'), temp.Find(x => x.Contains("apellido1")).Split(':')[1].Trim('"'), temp.Find(x => x.Contains("apellido2")).Split(':')[1].Trim('"'), curp, temp.Find(x => x.Contains("apellido1")).Split(':')[1].Trim('"'), temp.Find(x => x.Contains("sexo")).Split(':')[1].Trim('"'));
               }
               else
                  throw new Exception("El servicio de RNAPU no respondió intentelo nuevamente");
            }
            return result;
         }
         catch (Exception ex)
         {
            throw new Exception(ex.Message);
         }
      }
      public static byte[] FromHex(string hex)
      {
         hex = hex.Replace("-", "").Replace(" ", "");
         byte[] raw = new byte[hex.Length / 2];
         for (int i = 0; i < raw.Length; i++)
         {
            raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
         }
         return raw;
      }
      public static object[] ObtieneFirma(MemoryStream Cer, MemoryStream Key, string pass, string hash256)
      {      
            //firma     
            // 1) Desencriptar la llave privada, el primer parametro es la contraseña de llave privada y el segundo es la llave privada en formato binario.
            AsymmetricKeyParameter asp = Org.BouncyCastle.Security.PrivateKeyFactory.DecryptKey(pass.ToCharArray(), Key.ToArray());
            // 2) Convertir a parámetros de RSA
            Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters key = (Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters)asp;
            // 3) Crear el firmador con SHA256
            ISigner sig = Org.BouncyCastle.Security.SignerUtilities.GetSigner("SHA256withRSA");
            // 4) Inicializar el firmador con la llave privada
            sig.Init(true, key);
            // 5) Pasar la cadena original a formato binario
            byte[] bytes = Encoding.UTF8.GetBytes(hash256);
            // 6) Encriptar
            sig.BlockUpdate(bytes, 0, bytes.Length);
            byte[] bytesFirmados = sig.GenerateSignature();
            // 7) Finalmente obtenemos el sello
            string firma = Convert.ToBase64String(bytesFirmados);
            var cx509 = new X509Certificate2(Cer.ToArray());
            var cert_user = new Org.BouncyCastle.X509.X509CertificateParser().ReadCertificate(cx509.GetRawCertData());
            Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters publicKey = (Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters)cert_user.GetPublicKey();
            return new object[] { key.Modulus.ToString() == publicKey.Modulus.ToString(), firma };
         
      }
      public static void CopyStream(Stream input, Stream output)
      {
         byte[] buffer = new byte[16 * 1024];
         int read;
         while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
         {
            output.Write(buffer, 0, read);
         }
      }
      public static iTextSharp.text.Image GeneraQR(string link)
      {
         try
         {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(3, Color.Black, Color.White, false);
            using (MemoryStream stream = new MemoryStream())
            {
               qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
               iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(stream.ToArray());
               return image;
            }

         }
         catch (Exception ex)
         {
            throw new Exception("Error al generar QR. " + ex.Message.ToString());
         }
      }
      private static Org.BouncyCastle.Asn1.X509.X509Extensions CreateExtension()
      {
         byte[] nonce = new byte[16];
         Hashtable exts = new Hashtable();
         Org.BouncyCastle.Math.BigInteger nc = Org.BouncyCastle.Math.BigInteger.ValueOf(DateTime.Now.Ticks);
         Org.BouncyCastle.Asn1.X509.X509Extension nonceext = new Org.BouncyCastle.Asn1.X509.X509Extension(false, new Org.BouncyCastle.Asn1.DerOctetString(nc.ToByteArray()));
         exts.Add(Org.BouncyCastle.Asn1.Ocsp.OcspObjectIdentifiers.PkixOcspNonce, nonceext);
         return new Org.BouncyCastle.Asn1.X509.X509Extensions(exts);
      }
      private static byte[] CreateOCSPPackage(Org.BouncyCastle.X509.X509Certificate cert, Org.BouncyCastle.X509.X509Certificate cacert)
      {
         Org.BouncyCastle.Ocsp.OcspReqGenerator gen = new Org.BouncyCastle.Ocsp.OcspReqGenerator();
         try
         {
            Org.BouncyCastle.Ocsp.CertificateID certId = new Org.BouncyCastle.Ocsp.CertificateID(CertificateID.HashSha1, cacert, cert.SerialNumber);
            gen.AddRequest(certId);
            gen.SetRequestExtensions(CreateExtension());
            Org.BouncyCastle.Ocsp.OcspReq req;
            req = gen.Generate();
            return req.GetEncoded();
         }
         catch (OcspException e)
         {
            Console.WriteLine(e.StackTrace);
         }
         catch (IOException e)
         {

            Console.WriteLine(e.StackTrace);
         }
         return null;
      }
      private static byte[] ToByteArray(Stream stream)
      {
         byte[] buffer = new byte[4096 * 8];
         MemoryStream ms = new MemoryStream();
         int read = 0;
         while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            ms.Write(buffer, 0, read);
         return ms.ToArray();
      }
      private static byte[] PostRequest(string url, byte[] data, string contentType, string accept)
      {
         HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
         request.Method = "POST";
         request.ContentType = contentType;
         request.ContentLength = data.Length;
         request.Accept = accept;
         Stream stream = request.GetRequestStream();
         stream.Write(data, 0, data.Length);
         stream.Close();
         HttpWebResponse response = (HttpWebResponse)request.GetResponse();
         Stream respStream = response.GetResponseStream();
         Console.WriteLine(string.Format("HttpStatusCode : {0}", response.StatusCode.ToString()));
         byte[] resp = ToByteArray(respStream);
         respStream.Close();
         return resp;
      }

      public static bool ValidateOCSP(Org.BouncyCastle.X509.X509Certificate cert, string ac_productivo, string url)
      {
         try
         {

            string patarchivos = @"Areas\FEA\Utility\Files\";
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
               patarchivos = @"Areas/FEA/Utility/Files/";
            
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Utility", "Files", $"AC{ac_productivo}_SAT.crt");
            byte[] fileBytes = File.ReadAllBytes(path);

            var cax509 = new X509Certificate2(fileBytes);

                var cacert = new Org.BouncyCastle.X509.X509CertificateParser()
             .ReadCertificate(cax509.GetRawCertData());

                byte[] packtosend = CreateOCSPPackage(cert, cacert);
            byte[] response = PostRequest(url, packtosend, "Content-Type", "application/ocsp-request");
            string respOCSP = VerifyResponse(response);
            if (respOCSP.Length > 1 && respOCSP == "good")
               return true;
            else
               return false;
         }
         catch (Exception ex)
         {
            throw new Exception("No cumple esquema. " + ex.Message.ToString());
         }
      }
      private static string VerifyResponse(byte[] response)
      {
         OcspResp r = new OcspResp(response);
         string cStatusEnum = "";
         switch (r.Status)
         {
            case OcspRespStatus.Successful:
               BasicOcspResp or = (BasicOcspResp)r.GetResponseObject();
               if (or.Responses.Length == 1)
               {
                  SingleResp resp = or.Responses[0];
                  Object certificateStatus = resp.GetCertStatus();
                  //this part returns  null actually 
                  if (certificateStatus == null)
                  {
                     Console.WriteLine("Status is null ! ");
                  }
                  if (certificateStatus == null || certificateStatus == Org.BouncyCastle.Ocsp.CertificateStatus.Good)
                  {
                     cStatusEnum = "good"; //CertificateStatusEnum.Good;
                  }
                  else if (certificateStatus is Org.BouncyCastle.Ocsp.RevokedStatus)
                  {
                     cStatusEnum = "Revoked"; //CertificateStatusEnum.Revoked;
                  }
                  else if (certificateStatus is Org.BouncyCastle.Ocsp.UnknownStatus)
                  {
                     cStatusEnum = "";//CertificateStatusEnum.Unknown;
                  }
               }
               break;
            default:
               cStatusEnum = "Status desconodico: '" + r.Status + "'.";
               break;
         }
         return cStatusEnum;
      }
      public static string GetSHA256HashFromFile(MemoryStream fileName)
      {
         SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();
         byte[] hashedBytes = provider.ComputeHash(fileName);
         StringBuilder output = new StringBuilder();
         for (int i = 0; i < hashedBytes.Length; i++)
            output.Append(hashedBytes[i].ToString("x2").ToLower());
         return output.ToString();
      }
      public static string GetSHA256HashFromFile(string fileName)
      {
         using (var filestream = new FileStream(fileName, FileMode.Open))
         {
            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();
            byte[] hashedBytes = provider.ComputeHash(filestream);
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
               output.Append(hashedBytes[i].ToString("x2").ToLower());
            return output.ToString();
         }
      }

        public static string GetSHA256HashFromStream(Stream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException(nameof(inputStream), "El flujo de entrada no puede ser nulo.");
            }

            if (!inputStream.CanRead)
            {
                throw new ArgumentException("El flujo de entrada no se puede leer.", nameof(inputStream));
            }

            try
            {
                using (var sha256 = new SHA256CryptoServiceProvider())
                {
                    byte[] hashedBytes = sha256.ComputeHash(inputStream);

                    StringBuilder output = new StringBuilder();
                    foreach (byte b in hashedBytes)
                    {
                        output.Append(b.ToString("x2").ToLower());
                    }

                    return output.ToString();
                }
            }
            catch (ObjectDisposedException ex)
            {
                throw new InvalidOperationException("El flujo ya ha sido cerrado o eliminado.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ocurrió un error al calcular el hash SHA-256.", ex);
            }
        }

  
        public static object GetDataToFormandobject(Type type, IFormCollection formulario)
      {
         List<PropertyInfo> propInfos = type.GetProperties().ToList();
         object result = Activator.CreateInstance(type);
         foreach (var item in propInfos)
         {
            PropertyInfo propidad = propInfos.Find(x => x.Name == item.Name);
            if (formulario.ContainsKey(item.Name))
            {

               if (propidad.PropertyType.Name != "DateTime")
                  propidad.SetValue(result, Convert.ChangeType(formulario[item.Name].ToString(), propidad.PropertyType));
               else
                  propidad.SetValue(result, DateTime.ParseExact(formulario[item.Name].ToString(), "dd/MM/yyyy", null));
            }
         }
         return result;
      }
      public static string Encrypt(string clearText)
      {
         string EncryptionKey = "abC+)YEjT<[3rL(N4Z($j<Fm";
         byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
         using (Aes encryptor = Aes.Create())
         {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
               using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
               {
                  cs.Write(clearBytes, 0, clearBytes.Length);
                  cs.Close();
               }
               clearText = Convert.ToBase64String(ms.ToArray());
            }
         }
         return clearText;
      }
      public static IEnumerable<T> ConsultaCatalogo<T>(Type type, IDbConnection connection, string conditional = null)
      {
         List<PropertyInfo> propInfos = type.GetProperties().ToList();
         string columnas = "";
         string tabla = (string)type.GetField("tabla").GetValue(null);
         string excepciones = null;
         if (type.GetField("excepciones") != null)
            excepciones = (string)type.GetField("excepciones").GetValue(null);
         foreach (var item in propInfos)
            if (excepciones == null || !(excepciones.Split(',').ToList<string>()).Exists(x => x.Equals(item.Name)))
               columnas += item.Name + ",";
         columnas = columnas.TrimEnd(',');
         if (conditional == null)
            conditional = " ";
         string oreden = " ;";
         if (type.GetField("orden") != null)
            oreden = " order by " + (string)type.GetField("orden").GetValue(null);
          return connection.Query<T>(" Select " + columnas + " from  " + tabla + " " + conditional + " " + oreden);
      }
      public static bool B64ToFile(string archivoB64, string nombre)
      {
         try
         {
            if (System.IO.File.Exists(nombre))
               System.IO.File.Delete(nombre);

            byte[] filebytes = Convert.FromBase64String(archivoB64);
            FileStream fs = new FileStream(nombre,
                                            FileMode.CreateNew,
                                            FileAccess.Write,
                                            FileShare.None);
            fs.Write(filebytes, 0, filebytes.Length);
            fs.Close();
            return true;
         }
         catch
         {

            return false;
         }
      }
      public static string Decrypt(string cipherText)
      {
         string EncryptionKey = "abC+)YEjT<[3rL(N4Z($j<Fm";
         cipherText = cipherText.Replace(" ", "+");
         byte[] cipherBytes = Convert.FromBase64String(cipherText);
         using (Aes encryptor = Aes.Create())
         {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
               using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
               {
                  cs.Write(cipherBytes, 0, cipherBytes.Length);
                  cs.Close();
               }
               cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
         }
         return cipherText;
      }
        public static bool userHabilitado(string usraccountctrl)
        {
            try
            {
                int numero = Int32.Parse(usraccountctrl);
                string binary = Convert.ToString(numero, 2);
                //Console.WriteLine("Binario: " + binary);
                if (binary.EndsWith("10") || binary.EndsWith("11"))
                {
                    return false;
                }
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }
        public static void delay(int Time_delay)
        {
            int i = 0;
            //  ameTir = new System.Timers.Timer();
            var  _delayTimer = new System.Timers.Timer();
            _delayTimer.Interval = Time_delay;
            _delayTimer.AutoReset = false; //so that it only calls the method once
            _delayTimer.Elapsed += (s, args) => i = 1;
            _delayTimer.Start();
            while (i == 0) { };
        }
    }
}
