using System.IO;

namespace FirmaDocumento.Areas.FEA.Models
{
   public class DocumentosPendientes
   {
      public string UREmisor { get; set; }
      public string URDestino { get; set; }
      public int DocumentId { get; set; }
      public string NombreArchivo { get; set; }
      public string IdAlfresco { get; set; }
      public string Oficio { get; set; }
      public string Asunto { get; set; }
      public string Destinatario{ get; set; }
      public string NombreFirmante1 { get; set; }
      public string FechaFirma1 { get; set; }
      public string NombreFirmante2 { get; set; }
      public string FechaFirma2 { get; set; }
      public string NombreFirmante3 { get; set; }
      public string FechaFirma3 { get; set; }
      public string NombreFirmante4 { get; set; }
      public string FechaFirma4 { get; set; }
      public int OrdenFirmante { get; set; }
      public int TurnoFirmante { get; set; }
      public int TotalFirmantes { get; set; }
      public string NombreEstatus { get; set; }
      public string TipoFirma { get; set; }

      public string EstatusId { get; set; }
      public string ClaveEstatus { get; set; }

      public string FechaAlta { get; set; }
      public string FechaAltaOrden { get; set; }
      public string FechaFirmaOrden1 { get; set; }
      public string FechaCancelacion { get; set; }
      public string MotivoCancelacion { get; set; }
      public string TipoFirmaId { get; set; }
      public string fechafirma { get; set; }
      public string UserAlta { get; set; }
      public string EstatusxFirmante { get; set; }
      public string Elaboro { get; set; }

   }
}
