using System.IO;

namespace FirmaDocumento.Areas.FEA.Models
{
   public class DatosDocumentoYFirmante
   {
      public string UREmisor { get; set; }
      public string URDestino { get; set; }
      public int DocumentId { set; get; }
      public string NombreArchivo { get; set; }
      public string IdAlfresco { get; set; }
      public string Oficio { get; set; }
      public string Asunto { set; get; }
      public string Tramite { set; get; }
      public string Destinatario { get; set; }
      public string EmailEmisor { set; get; }
      public string EmailDestinatarios { get; set; }
      public string NombreFirmante1 { get; set; }
      public string EmailFirmante1 { get; set; }
      public string FechaFirma1 { get; set; }
      public string NombreFirmante2 { get; set; }
      public string EmailFirmante2 { get; set; }
      public string FechaFirma2 { get; set; }
      public string NombreFirmante3 { get; set; }
      public string EmailFirmante3 { get; set; }
      public string FechaFirma3 { get; set; }
      public string NombreFirmante4 { get; set; }
      public string EmailFirmante4 { get; set; }
      public string FechaFirma4 { get; set; }
      public string EmailFirmante { get; set; }
      public int OrdenFirmante { get; set; }
      public int TurnoFirmante { get; set; }
      public int TotalFirmantes { get; set; }
      public string NombreDestinatario { get; set; }
      public string UserAlta { get; set; }
      public string Observaciones  { get; set; }
      public int id_unidad_destino { get; set; }
      public int id_unidad_remitente { get; set; }
      public string TipoFirma { get; set; }
      public string NombreEstatus { get; set; }
      public string EstatusId { get; set; }
      public string ClaveEstatus { get; set; }
      public string FechaCancelacion { get; set; }
      public string MotivoCancelacion { get; set; }
      public string Elaboro { get; set; }
      public string SuplenciaDe { get; set; }
      public string Tipo_Rechazo { get; set; }
      /*
      public string Pass { get; set; }
      public MemoryStream File { get; set; }
      public MemoryStream CER { get; set; }
      public MemoryStream KEY { get; set; }
      */
   }
}