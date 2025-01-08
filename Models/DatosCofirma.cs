using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace APIEfirma.Models
{
   public class DatosCofirma
   {
      public string UREmisor { get; set; }
      public string URDestino { get; set; }
      public string Oficio { get; set; }
      public string Asunto { set; get; }
      public string Tramite { set; get; }
      public string idUnidadDest { set; get; }
      public string Destinatario { get; set; }
      public int DocumentId { set; get; }
      public string AlfrescoId { set; get; }
      public string NombreFirmante { set; get; }
      public string EmailEmisor { set; get; }
      public string EmailFirmante { set; get; }
      public string EmailDestinatarios { set; get; }
      public string NombreArchivo { set; get; }
      //public string CURP { set; get; }
      public string RFC { set; get; }
      public string Pass { set; get; }
      public string Observaciones { set; get; }
      public string CadenaOriginal { set; get; }
      public string CadenaFirmada { set; get; }
      public string Firma { set; get; }
      public string SerialFirma { set; get; }
      public string SelloFirma { set; get; }
      public DateTime FechaFirma { set; get; }
      public DateTime FechaVigencia { set; get; }
      public DateTime FechaUTC { set; get; }
      public string AutoridadCertificadora { set; get; }
      public int NumeroTotalPaginas { set; get; }
      public int OrdenFirmante { set; get; }
      public int TurnoFirmante { set; get; }
      public int TotalFirmantes { set; get; }
      public bool Firmado { get; set; }
      public bool FirmasConcluidas { get; set; }
      //public MemoryStream File { get; set; }
      public MemoryStream CER { get; set; }
      public MemoryStream KEY { get; set; }
      public IFormFile PDF { get; set; }
      public string NombreDestinatario { get; set; }
      public string UserAlta { get; set; }
      public string sListIdDocs { get; set; }
      public string Motivo { get; set; }
      public string fechaCancelacion { get; set; }
      public string TipoCancelacion { get; set; }
      public string UserName { get; set; }
      public string NombreEstatus { get; set; }
      public string UsuariosRevisores { get; set; }
      public string Elaboro { get; set; }
      public string SuplenciaDe { get; set; }
      public string TipoRechazo { get; set; }

        public byte[] ArchivoBase { get; set; }
    }
}
