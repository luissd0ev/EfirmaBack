namespace APIEfirma.Interfaces
{
    public interface ICertificadoService
    {
        public Tuple<Boolean, String> ValidaCertSAT(MemoryStream certificado);
    }
}
