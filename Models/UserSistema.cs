using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaDocumento.Areas.FEA.Models
{
    public class UserSistema
    {
        public const string tabla = "schema_seguridad.td_user_sistema";
        public const string primaryKey = "tus_id_user";
        public const string orden = "tus_id_user";
        public const string excepciones = "Identificacion,TokenUserSession,Time,NombreCompleto,cu_nombres,cu_primer_apellido,cu_segundo_apellido,cu_curp,menu,cu_puesto"; //exclución de cu_puesto de la db";
        public int? tus_id_user { set; get; }
        public int tus_id_rol { set; get; }
        public string tus_nombre_user { set; get; }
        public string tus_correo_electronico { set; get; }
        public string tus_password { set; get; }
        public string Identificacion { set; get; }
        public string TokenUserSession { set; get; }
        public DateTime Time { set; get; }
        public string cu_nombres { set; get; }
        public string NombreCompleto { set; get; }
        public string cu_primer_apellido { set; get; }
        public string cu_segundo_apellido { set; get; }
        public string cu_curp { set; get; }
        public string cu_unidad { set; get; }
        public bool tus_activo { set; get; }
        public bool cu_puesto { set; get; } //Creación de una propiedad para control de logeo
        public string tus_correo_control { set; get; } //Creación de una propiedad para control de logeo
        public string cu_RFC { get; set; }
    }
}
