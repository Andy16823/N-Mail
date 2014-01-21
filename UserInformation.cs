using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nMail
{
    /// <summary>
    /// Diese Klasse fungiert als konten klasse
    /// </summary>
    public class UserInformation : System.Net.NetworkCredential
    {

        public int Port { get; set; }
        public Boolean SSL { get; set; }

        /// <summary>
        /// Initialisiert eine Neue Klasse UserInformationen
        /// </summary>
        /// <param name="username">Benutzername</param>
        /// <param name="Server">Server Adresse</param>
        /// <param name="Password">Passwort</param>
        /// <param name="Port">Server Port</param>
        /// <param name="SSL">SSL Verbindung Nutzen</param>
        public UserInformation(String username, String Server, String Password, int Port,Boolean SSL)
        {
            this.Domain = Server;
            this.Password = Password;
            this.UserName = username;
            this.Port = Port;
            this.SSL = SSL;
        }

        
        
        
    }
}
