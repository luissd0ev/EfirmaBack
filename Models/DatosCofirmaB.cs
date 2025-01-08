namespace APIEfirma.Models
{
    public class DatosCofirmaB
    {
        public byte[] CER { get; set; } // Certificado digital en formato byte array.
        public byte[] KEY { get; set; } // Llave privada asociada al certificado.
        public string Pass { get; set; } // Contraseña para la llave privada.
        public string NombreArchivo { get; set; } // Nombre del archivo que se firmará.
        public byte[] CadenaOriginal { set; get; }
        public string Firma { set; get; }

        public string SerialFirma { set; get; }
    }
}
