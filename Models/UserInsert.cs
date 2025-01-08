using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaDocumento.Areas.FEA.Models
{
    public class UserInsert
    {
        private string username;
        private string userpassword;
        private string usercurp;
        private string usermail;
        private int iduser;
        public string Username { get => username; set => username = value; }
        public string Userpassword { get => userpassword; set => userpassword = value; }
        public string Usercurp { get => usercurp; set => usercurp = value; }
        public string Usermail { get => usermail; set => usermail = value; }
        public int Iduser { get => iduser; set => iduser = value; }

        public UserInsert()
        {
        }
      
    }
}
