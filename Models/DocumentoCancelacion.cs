using System;
using System.Collections.Generic;
using System.Linq;

namespace FirmaDocumento.Areas.FEA.Models
{
   public class DocumentoCancelacion
   {
      public int DocumentId { get; set; }
      public string UserRFC { get; set; }
      public string UserCancela { get; set; }
      public string NombreCancela { get; set; }
      public string IdAlfresco { get; set; }
      public string Motivo { set; get; }
      public string TipoCancelacion { set; get; }
      public string TipoRechazo { set; get; }
   }
}
