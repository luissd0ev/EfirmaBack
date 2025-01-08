using System;
using System.ComponentModel.DataAnnotations;

namespace FirmaDocumento.Areas.FEA.Models
{
    public class Documentos
    {
        public int td_anio_docto { set; get; }
        public int td_id_firma { get; set; }
        public int cur_id { set; get; }
        public string cur_descripcion { set; get; }

        public string td_referencia { set; get; }
        public string td_id_unidadr_emisora { set; get; }
        public string unidad_emisora { set; get; }
        public string td_id_unidadr_destino { set; get; }
        public string clave_unidad_destino { set; get; }
        public string unidad_destino { set; get; }
        public string td_asunto { set; get; }
        public string td_nombre_doctos { set; get; }
        public string td_nombre_destinatario { set; get; }
        public string td_nombre_firmante { set; get; }
        public string td_idalfresco { set; get; }
        public string td_observaciones { set; get; }
        public string td_destinatarios { set; get; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? td_fecha { set; get; }
        public string td_usr_alta { set; get; }
        public string td_tramite { set; get; }


    }
}
