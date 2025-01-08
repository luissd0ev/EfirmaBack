using System.IO;

namespace FirmaDocumento.Areas.FEA.Models
{
   public class DatosFirma
   {
      public int Tipodocumento { set; get; }
      public string UnidadResponsableEmisora { set; get; }
      public string UnidadResponsableDestino { set; get; }
      public string Tramite { set; get; }
      public string Asunto { set; get; }
      public string Pass { set; get; }
      public string NombreDestinatario { set; get; }
      public string NombreArchivo { set; get; }
      public string nombre { set; get; }
      public string MailEmisor { set; get; }
      public string MailDestino1 { set; get; }
      public string MailDestino2 { set; get; }
      public string MailDestinocontrol { set; get; }
      public string MailControlEmisor { set; get; }
      public string Referencia { set; get; }
      public string Observaciones { set; get; }
      public string RegistrosDestino { get; set; }
      public string CorreosDestinatarios { get; set; }
      public bool Cofirma { get; set; }
      public bool Firma { get; set; }
      public MemoryStream File { get; set; }
      public MemoryStream CER { get; set; }
      public MemoryStream KEY { get; set; }
      public string UserAlta { get; set; }
      public bool CofirmaInmediata { get; set; }
      public int tfi_id { get; set; }
      public string UsuariosRevisores { get; set; }
      public string Elaboro { get; set; }
      public string suplenciaDe { get; set; }
   }
}