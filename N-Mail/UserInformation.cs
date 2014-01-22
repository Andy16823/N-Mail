using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nMail
{
    /// <summary>
    /// Diese Klasse fungiert als Kontenklasse
    /// </summary>
    public class UserInformation : System.Net.NetworkCredential
    {

        public int Port { get; set; }
        public Boolean SSL { get; set; }

        /// <summary>
        /// Initialisiert die Benutzerinformationen
        /// </summary>
        /// <param name="username">Benutzername</param>
        /// <param name="Server">Serveradresse</param>
        /// <param name="Password">Passwort</param>
        /// <param name="Port">Serverport</param>
        /// <param name="SSL">SSL-Verbindung nutzen?</param>
        public UserInformation(String username, String server, String password, int port, Boolean ssl)
        {
            this.Domain = server;
            this.Password = password;
            this.UserName = username;
            this.Port = port;
            this.SSL = ssl;
        }
    }
}
