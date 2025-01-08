using System;

namespace FirmaDocumento.Areas.FEA.Models
{
    public class UnidadResponsable
    {
        public const string tabla = "schema_catalogos.cat_unidades_responsables";
        public const string primaryKey = "cur_id";
        public const string orden = "cur_cve_unidad";
        public int cur_id { set; get; }
        public string cur_descripcion { set; get; }
        public DateTime cur_fecha_alta { set; get; }
        public bool cur_estatus { set; get; }
        public string cur_cve_unidad { set; get; }
        public string cur_correo_vu { set; get; }
        public bool cur_unidad_externa { set; get; }
   }
}
