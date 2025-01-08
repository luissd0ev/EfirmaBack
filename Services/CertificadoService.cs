using APIEfirma.Interfaces;
using APIEfirma.Utility;
using EFirmaProyecto.Utility;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace APIEfirma.Services
{
    public class CertificadoService:ICertificadoService
    {

        public string serie_firmante { get; set; }
        private string ac_productivo = "";
        private string web_SAT = conf["DirWork:web_SAT"].ToString();
        private static IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: false).Build());
        private string time_call = conf["DirWork:time_call"].ToString();

        public Tuple<Boolean, String> ValidaCertSAT(MemoryStream certificado)
        {
            X509Certificate2 x509 = new X509Certificate2(certificado.ToArray());
            serie_firmante = Encoding.ASCII.GetString(Tools.FromHex(x509.SerialNumber));
            ac_productivo = serie_firmante.Substring(11, 1);
            var cert_user = new Org.BouncyCastle.X509.X509CertificateParser().ReadCertificate(x509.GetRawCertData());

            //Condiciones y recorrido para validar certificado con el SAT hasta 3 veces
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    if (Tools.ValidateOCSP(cert_user, ac_productivo, web_SAT))
                    {
                        return new Tuple<Boolean, String>(true, "");
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Log log = new Log("ErrorLog");
                    log.Add(ex.Message + " - " + ex.StackTrace);
                    if (ex.Message.Contains("The remote server returned an error: (502)"))
                    {
                        Tools.delay(Int32.Parse(time_call));
                    }
                    if (i == 2)
                    {
                        return new Tuple<Boolean, String>(false, "Error- Error en la comunicación con el SAT");
                    };
                }

            }
            return new Tuple<Boolean, String>(false, "Error- El certificado no es admitido por el Sat");
        }


    }
}
