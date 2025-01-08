using System;

namespace FirmaDocumento.Areas.FEA.Models
{
    public class Cofirma
    {
        public const string tabla = "schema_firma.td_cofirma";
        public const string primaryKey = "cof_id";
        public const string orden = "cof_id";
        public int cof_id { set; get; }
        public string cof_nombre_firmante{ set; get; }
        public string cof_curp { set; get; }
        public string cof_rfc { set; get; }
        public string cof_correo { set; get; }
        public string cof_cur_cve_unidad { set; get; }
        public string cof_serial { set; get; }
        public string cof_cadena_original { set; get; }
        public string cof_firma { set; get; }
        public DateTime cof_fecha_firma { set; get; }
        public int cof_orden { set; get; }
        public int cof_td_id_firma { set; get; }
    }
}
