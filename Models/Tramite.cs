namespace FirmaDocumento.Areas.FEA.Models
{
    public class Tramite
    {
        public const string tabla = "schema_catalogos.cat_tramite ct";
        public const string primaryKey = "ctt_id_tramite";
        public const string orden = "ctt_id_tramite";
        public int ctt_id_tramite { set; get; }
        public string ctt_tramite { set; get; }
        public string ctt_descripcion { set; get; }
    }
}
