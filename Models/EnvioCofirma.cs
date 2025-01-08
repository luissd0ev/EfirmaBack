using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaDocumento.Areas.FEA.Models
{
   public class EnvioCofirma
   { 
      public int IdDocumento { get; set; }
      public string idalfresco { set; get; }
      public string idalfresco_sinfirma { set; get; }
      public string nombre_doctos { set; get; }
      public string asunto { set; get; }
      public string nombre_destinatario { set; get; }
        public string id_unidad_destino { set; get; } 
        public string tramite { set; get; }
        public string referencia { set; get; }
        public string curp { set; get; }
      public string rfc { set; get; }
      public string observaciones { set; get; }
      public string cadena_original { set; get; }
      public string cadena_firmada { set; get; }
      public string firma { set; get; }
      public string serial_firma { set; get; }
      public string sello_firma { set; get; }
      public string fecha_firma { set; get; }
      public string fecha_vigencia { set; get; }
      public string fecha_UTC { set; get; }
      public string autoridad_certificadora { set; get; }
      public int numero_paginas { set; get; }
      public bool firmado { set; get; }
      public bool firmasConcluidas { set; get; }
      public string elaboro { set; get; }

   }
}
