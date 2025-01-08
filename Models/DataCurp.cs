using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIEfirma.Models
{
    public class DataCurp
    {
        private string nombres;
        private string apellidoP;
        private string apellidoM;
        private string curp;
        private string email;
        private string genero;
        public string Nombres { get => nombres; set => nombres = value; }
        public string ApellidoP { get => apellidoP; set => apellidoP = value; }
        public string ApellidoM { get => apellidoM; set => apellidoM = value; }
        public string Curp { get => curp; set => curp = value; }
        public string Email { get => email; set => email = value; }
        public string Genero { get => genero; set => genero = value; }

        public DataCurp(string nombres, string apellidoP, string apellidoM, string curp, string email, string genero)
        {
            this.nombres = nombres;
            this.apellidoP = apellidoP;
            this.apellidoM = apellidoM;
            this.curp = curp;
            this.email = email;
            this.Genero = genero;
        }
        public DataCurp(string nombres, string apellidoP, string apellidoM, string curp, string genero)
        {
            this.nombres = nombres;
            this.apellidoP = apellidoP;
            this.apellidoM = apellidoM;
            this.curp = curp;
            this.Genero = genero;
        }
        public DataCurp()
        {

        }

    }
}
