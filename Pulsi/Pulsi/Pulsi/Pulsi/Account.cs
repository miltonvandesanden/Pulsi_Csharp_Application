using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulsi
{
    class Account
    {
        //fields
        private string username;
        private string password;

        //Properties
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        //methods
        public Account(string gebruikersnaam, string wachtwoord)
        {
            this.username = gebruikersnaam;
            this.password = wachtwoord;
        }
    }
}
