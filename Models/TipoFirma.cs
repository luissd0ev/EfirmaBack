using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaDocumento.Areas.FEA.Models
{
   public class TipoFirma
   {
      public const string tabla = "schema_catalogos.cat_tipos_firmas";
      public const string primaryKey = "tfi_id";
      public const string orden = "tfi_clave";
      public int tfi_id { set; get; }
      public string tfi_tipo_firma { set; get; }
      public string tfi_clave { set; get; }
   }
}
