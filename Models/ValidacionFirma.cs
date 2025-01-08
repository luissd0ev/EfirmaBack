using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaDocumento.Areas.FEA.Models
{
    public class ValidacionFirma
    {
        public DateTime td_fecha { get; set; }
        public string cur_descripcion_e { get; set; }
        public string cur_descripcion_d { get; set; }
        public string td_nombre_firmante { get; set; }
        public string td_curp { get; set; }
        public string td_rfc { get; set; }
        public string td_nombre_destinatario { get; set; }
        public string td_serial_firma { get; set; }
        public string cur_cve_unidad_e { get; set; }
        public string cur_cve_unidad_d { get; set; }
        public DateTime td_fecha_firma { get; set; }
        public DateTime td_fecha_vigencia { get; set; }
        public DateTime td_fecha_utc { get; set; }
        public string td_autoridad_certificadora { get; set; }
        public string td_cadena_original { get; set; }
        public string td_numero_total_paginas { get; set; }
        public string td_firma { get; set; }
        public string UserAlta { get; set; }
        public string eso_clave { get; set; }
        public string td_orden { get; set; }
        
    }
}
