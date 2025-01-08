using System;
namespace FirmaDocumento.Areas.FEA.Models
{
    public class EnvioOficio
    {
        public string referencia { set; get; }
        public string unidadr_emisora { set; get; }
        public string unidadr_destino { set; get; }
        public string asunto { set; get; }
        public string nombre_doctos { set; get; }
        public string nombre_destinatario { set; get; }
        public string nombre_firmante { set; get; }
        public string serial_firma { set; get; }
        public string curp { set; get; }
        public string rfc { set; get; }
        public string cadena_original { set; get; }
        public string cadena_firmada { set; get; }
        public string firma { set; get; }
        public string idalfresco { set; get; }
        public string observaciones { set; get; }
        public string destinatarios { set; get; }
        public string usr_alta { set; get; }
        public string tramite { set; get; }
        public bool cofirma { set; get; }
        public bool firmasConcluidas  { set; get; }
		  public DateTime? fecha_firma { get; set; }
        public DateTime? fecha_vigencia { get; set; }
        public DateTime? fecha_utc { get; set; }
        public string correo_emisor { get; set; }
        public string correo_control_emisor { get; set; }
        public string autoridad_certificadora { get; set; }
        public int numero_total_paginas { get; set; }
        public string correo_firmante { get; set; }
        public int tfi_id { get; set; }
        public int total_firmantes { get; set; }
        public string elaboro { get; set; }
        public string suplenciaDe { get; set; }
        public int turno_firmante { get; set; }

   }
}
