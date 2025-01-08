
using APIEfirma.Models;
using static APIEfirma.Controllers.EfirmaController;

namespace APIEfirma.Interfaces
{
    public interface IFirmaService
    {
        public FirmaResultado GeneraCadenaFirma(DatosCofirmaB datosBaseDoc, MemoryStream documentoStream);

        public MemoryStream EscribeFirma(
 string firma,
 string hash256,
 DatosCofirmaB datosBaseDoc,
 string firmante,
 string fechacdmx,
 int ordenFirmante,
 int totalFirmantes,
 bool shouldMaskSensitiveData);

        public string ExtraerFirmaDesdePdf(byte[] documentoBytes);
    }
}
