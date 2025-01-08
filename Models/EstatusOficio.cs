using System;

namespace FirmaDocumento.Areas.FEA.Models
{
    public class EstatusOficio
    {
        public const string tabla = "schema_catalogos.cat_estatus_oficios";
        public const string primaryKey = "eso_id";
        public const string orden = "eso_id";
        public int eso_id { set; get; }
        public string eso_nombre_estatus{ set; get; }
        public string eso_clave { set; get; }
   }
}
