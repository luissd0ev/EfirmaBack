using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaDocumento.Areas.FEA.Models
{
   public class UnidadExterna
   {
      public const string tabla = "schema_catalogos.cat_unidades_responsables";
      public const string primaryKey = "cur_id";
      public const string orden = "cur_cve_unidad";
      public int cur_id { set; get; }
      public string cur_descripcion { set; get; }
      public string cur_cve_unidad { set; get; }
   }
}
