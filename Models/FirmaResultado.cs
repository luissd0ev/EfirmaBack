namespace APIEfirma.Models
{
    public class FirmaResultado
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public string fecha_revo { get; set; }
        public string hash256 { get; set; }
        public string firma { get; set; }
        public string fechacdmx { get; set; }
        public string fecha_utc { get; set; }
        public byte[] CadenaOriginal { get; set; }

        public string firmanteDocumento { get; set; }
    }
}
