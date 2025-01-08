using APIEfirma.Models;
using APIEfirma.Utility;
using static APIEfirma.Controllers.EfirmaController;
using System.Security.Cryptography.X509Certificates;
using APIEfirma.Interfaces;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;

namespace APIEfirma.Services
{
    public class FirmaService:IFirmaService
    {
        public FirmaResultado GeneraCadenaFirma(DatosCofirmaB datosBaseDoc, MemoryStream documentoStream)
        {
            var resultado = new FirmaResultado();

            try
            {
                // Validar la vigencia del certificado
                var x509 = new X509Certificate2(datosBaseDoc.CER);
                resultado.fecha_revo = x509.NotAfter.ToString("dd/MM/yyyy hh:mm tt");

                if (!(DateTime.Today >= x509.NotBefore && DateTime.Today <= x509.NotAfter))
                {
                    resultado.IsError = true;
                    resultado.ErrorMessage = $"CERTIFICADO VENCIDO. Fecha de vencimiento: {x509.NotAfter}";
                    return resultado;
                }

                // Obtener datos del certificado
                var nombre_cer = x509.Subject.ToString()
                    .Replace("subject=", "")
                    .Replace(", ", ",")
                    .Replace("CN = ", "CN=")
                    .Split(new char[] { ',' });
                var firmante = Array.Find(nombre_cer, element => element.StartsWith("CN=", StringComparison.Ordinal))
                    .Replace("CN=", "");

                // Crear MemoryStreams a partir de los datos del certificado y la llave privada
                using var cerStream = new MemoryStream(datosBaseDoc.CER);
                using var keyStream = new MemoryStream(datosBaseDoc.KEY);

                // Calcular el hash directamente desde el MemoryStream
                documentoStream.Position = 0; // Asegurar que el stream esté al inicio
                var hash256 = Tools.GetSHA256HashFromStream(documentoStream);

                // Validar firma con llave privada
                var temp = Tools.ObtieneFirma(cerStream, keyStream, datosBaseDoc.Pass, hash256);

                if (!(bool)temp[0])
                {
                    resultado.IsError = true;
                    resultado.ErrorMessage = "EL CERTIFICADO (.cer) NO CORRESPONDE CON SU LLAVE PRIVADA (.key)";
                    return resultado;
                }

                // Asignar resultados
                resultado.hash256 = hash256;
                resultado.firma = temp[1].ToString();
                resultado.fechacdmx = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                resultado.fecha_utc = DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm tt");
                resultado.firmanteDocumento = firmante;

                return resultado;
            }
            catch (Exception ex)
            {
                resultado.IsError = true;
                resultado.ErrorMessage = $"Error inesperado: {ex.Message}";
                return resultado;
            }
        }
       
        public MemoryStream EscribeFirma(
    string firma,
    string hash256,
    DatosCofirmaB datosBaseDoc,
    string firmante,
    string fechacdmx,
    int ordenFirmante,
    int totalFirmantes,
    bool shouldMaskSensitiveData) // Nueva variable
        {
            MemoryStream sPDFStream = new MemoryStream();

            try
            {
                if (datosBaseDoc == null || datosBaseDoc.CadenaOriginal == null)
                {
                    throw new Exception("Los datos de firma están incompletos.");
                }

                using (var readerMemoryStream = new MemoryStream(datosBaseDoc.CadenaOriginal))
                {
                    PdfReader reader = new PdfReader(readerMemoryStream);
                    PdfStamper stamper = null;

                    try
                    {
                        stamper = new PdfStamper(reader, sPDFStream);
                        int totalPages = reader.NumberOfPages;

                        for (int i = 1; i <= totalPages; i++)
                        {
                            var contentByte = stamper.GetOverContent(i);
                            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.EMBEDDED);

                            contentByte.BeginText();
                            contentByte.SetFontAndSize(baseFont, 7);

                            string leyendaFirma1 = $"Archivo firmado por: {firmante} | Fecha de firma: {fechacdmx}";
                            string leyendaFirma2 = $"Firmante {ordenFirmante}/{totalFirmantes} | ID documento: {hash256} | Página: {i}";

                            bool isLandscape = reader.GetPageSize(i).Width > reader.GetPageSize(i).Height;
                            float x1, y1, x2, y2, rotation;

                            switch (ordenFirmante)
                            {
                                case 1:
                                    x1 = reader.GetPageSize(i).Width - 25;
                                    y1 = reader.GetPageSize(i).Top - 25;
                                    x2 = x1 - 10;
                                    y2 = y1;
                                    rotation = 270;
                                    break;
                                case 2:
                                    x1 = 25;
                                    y1 = 25;
                                    x2 = 25;
                                    y2 = 15;
                                    rotation = 0;
                                    break;
                                case 3:
                                    x1 = 25;
                                    y1 = 25;
                                    x2 = x1 + 10;
                                    y2 = y1;
                                    rotation = 90;
                                    break;
                                case 4:
                                    x1 = 25;
                                    y1 = reader.GetPageSize(i).Top - 15;
                                    x2 = x1;
                                    y2 = y1 - 10;
                                    rotation = 0;
                                    break;
                                default:
                                    throw new Exception("Orden de firmante no válido.");
                            }

                            DibujaFirma(isLandscape, ordenFirmante, contentByte, leyendaFirma1, leyendaFirma2, x1, y1, x2, y2, rotation);

                            contentByte.EndText();
                        }

                        stamper.InsertPage(totalPages + 1, reader.GetPageSize(1));
                        var newPageContentByte = stamper.GetOverContent(totalPages + 1);
                        var newPageFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.EMBEDDED);
                        newPageContentByte.BeginText();
                        newPageContentByte.SetFontAndSize(newPageFont, 10);

                        float yPosition = reader.GetPageSize(1).Height - 30;

                        newPageContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Autoridad certificadora: " + firmante, 50, yPosition - 20, 0);

                        string finalHash256 = shouldMaskSensitiveData
                            ? new string('X', Math.Min(29, hash256.Length)) + hash256.Substring(Math.Min(29, hash256.Length))
                            : hash256;

                        newPageContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "ID documento: " + finalHash256, 50, yPosition - 35, 0);
                        newPageContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Número total de páginas: " + (totalPages + 1).ToString(), 50, yPosition - 50, 0);

                        newPageContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "El presente documento ha sido firmado mediante el uso de la firma electrónica avanzada, amparada por un", 50, yPosition - 70, 0);
                        newPageContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "certificado digital vigente a la fecha de su elaboración", 50, yPosition - 85, 0);

                        newPageContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Firma:", 50, yPosition - 110, 0);

                        float firmaYPosition = yPosition - 125;
                        int maxLineLength = 80;

                        string finalFirma = shouldMaskSensitiveData
                            ? new string('X', Math.Min(15, firma.Length)) + firma.Substring(Math.Min(15, firma.Length))
                            : firma;

                        for (int startIndex = 0; startIndex < finalFirma.Length; startIndex += maxLineLength)
                        {
                            string firmaLine = finalFirma.Substring(startIndex, Math.Min(maxLineLength, finalFirma.Length - startIndex));
                            newPageContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, firmaLine, 50, firmaYPosition, 0);
                            firmaYPosition -= 15;
                        }

                        newPageContentByte.EndText();
                    }
                    finally
                    {
                        stamper?.Close();
                        reader.Close();
                    }
                }

                sPDFStream.Position = 0;
                return sPDFStream;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al firmar el documento: " + ex.Message);
            }
        }


        private void DibujaFirma(bool bPageLandscape, int iTurnoFirmante, PdfContentByte contentByte, string sLeyendaFirma1, string sLeyendaFirma2, float x1, float y1, float x2, float y2, float rot)
        {
            if ((bPageLandscape & (iTurnoFirmante == 1 || iTurnoFirmante == 3))
              || (!bPageLandscape & (iTurnoFirmante == 2 || iTurnoFirmante == 4)))//Si es documento horizontal se parte la línea del primer firmante.
            {
                contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, sLeyendaFirma1, x1, y1, rot);
                contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, sLeyendaFirma2, x2, y2, rot);
            }
            else if ((!bPageLandscape & (iTurnoFirmante == 1 || iTurnoFirmante == 3))
                 || (bPageLandscape & (iTurnoFirmante == 2 || iTurnoFirmante == 4)))
            {
                contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, sLeyendaFirma1 + "  " + sLeyendaFirma2, x1, y1, rot);
            }
        }


        public string ExtraerFirmaDesdePdf(byte[] documentoBytes)
        {
            try
            {
                using (var stream = new MemoryStream(documentoBytes))
                using (var pdfDocument = UglyToad.PdfPig.PdfDocument.Open(stream))
                {
                    foreach (var page in pdfDocument.GetPages())
                    {
                        string textoPagina = page.Text;

                        var match = Regex.Match(textoPagina, @"Firma:\s*([\w+/=]+)");

                        if (match.Success)
                        {
                            return match.Groups[1].Value;
                        }
                    }
                }

                throw new Exception("No se encontró ninguna firma en el documento.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al procesar el PDF: " + ex.Message);
            }
        }


    }
}
