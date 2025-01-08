using System;
using System.ComponentModel.DataAnnotations;

namespace FirmaDocumento.Areas.FEA.Models
{
    public class Revisor
    {
        public const string tabla = "schema_firma.td_revisor";
        public const string primaryKey = "td_rev_id";
        public const string orden = "td_rev_id";
        public int td_rev_id { set; get; }
        public int td_id_envio_oficios { set; get; }
        public string td_nombre_revisor{ set; get; }
        public string td_correo { set; get; }
        public string td_rfc { set; get; }
        public string td_cur_cve_unidad { set; get; }
        public string unidad_descripcion { set; get; }
        public int td_orden { set; get; }
    }
}
