using System;
using System.ComponentModel.DataAnnotations;

namespace FirmaDocumento.Areas.FEA.Models
{
    public class Firmante
    {
        public const string tabla = "schema_firma.td_firmante";
        public const string primaryKey = "td_id";
        public const string orden = "td_id";
        public int td_id { set; get; }
        public int td_id_envio_oficios { set; get; }
        public string td_nombre_firmante{ set; get; }
        public string td_curp { set; get; }
        public string td_rfc { set; get; }
        public string td_correo { set; get; }
        public string td_cur_cve_unidad { set; get; }
        public string td_serial_firma { set; get; }
        public string td_cadena_original { set; get; }
        public string td_firma { set; get; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        public DateTime? td_fecha_firma { set; get; }
        public DateTime? td_fecha_vigencia { set; get; }
        public DateTime? td_fecha_utc { set; get; }
        public int td_orden { set; get; }
        public string td_autoridad_certificadora { set; get; }
        public int td_numero_total_paginas { set; get; }
    }
}
