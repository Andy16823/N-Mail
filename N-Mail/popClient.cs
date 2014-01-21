using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace nMail
{
    public class popClient
    {
        private System.Net.Sockets.TcpClient client;
        private System.IO.StreamReader sr;
        private System.IO.StreamWriter sw;


        private void initial(UserInformation ui, System.IO.StreamReader sr, System.IO.StreamWriter sw)
        {
            // Wilkommens Nachricht
            sr.ReadLine();
            // Username
            sw.WriteLine("USER" + ui.Domain);
            // Leerlesen
            sr.ReadLine();
            // Passwort
            sw.WriteLine("PASS " + ui.Password);
            // leer leden
            sr.ReadLine();
        }

        /// <summary>
        /// Gibt die Anzahl der Mails zurück
        /// </summary>
        /// <param name="userinformation"></param>
        /// <returns></returns>
        public int getMailCount(UserInformation userinformation)
        {
            String WlkMsg;
            String msg;

            client = new TcpClient(userinformation.Domain, userinformation.Port);
            
            // Connecten
            connect(userinformation, client);

            // Autoflush
            sw.AutoFlush = true;

            // Wilkommens Nachricht
            WlkMsg = sr.ReadLine();

            // User Name Übergeben
            sw.WriteLine("USER " + userinformation.UserName);

            // leer lesen
            sr.ReadLine();

            // Passwort Senden
            sw.WriteLine("PASS " + userinformation.Password);
            
            // leer lesen
            sr.ReadLine();

            // Status
            sw.WriteLine("STAT");

            // leer lesen
            msg = sr.ReadLine();

            // String Spliten
            String[] match = msg.Split(' ');

            // String umwandeln in int
            int count = int.Parse(match[1]);

            return count;

            sw.WriteLine("QUIT");

            // Stream Schließen
            sw.Close();
            sr.Close();
            
        }
        
        /// <summary>
        /// Stellt eine Verbindung zum Server her.
        /// </summary>
        /// <param name="ui">Klasse UserInformation</param>
        /// <param name="reader">Reader</param>
        /// <param name="writer">Writer</param>
        /// <param name="client">TcpClient</param>
        public void connect(UserInformation ui, System.Net.Sockets.TcpClient client)
        {          
            // Prüft ob ein SSL Stream Verwendet werden soll.
            if(ui.SSL==true)
            {
                SslStream str = new SslStream(client.GetStream());
                str.AuthenticateAsClient(ui.Domain);
                sr = new System.IO.StreamReader(str);
                sw = new System.IO.StreamWriter(str);
            }
            else if(ui.SSL==false)
            {
                System.IO.Stream str = client.GetStream();
                sr = new System.IO.StreamReader(str);
                sw = new System.IO.StreamWriter(str);
            }
        }
        
        /// <summary>
        /// Gibt eine Mail vom Pop3 Server
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<String> getMail(UserInformation ui, int n)
        {
            String tmps;
            List<String> Nachricht = new List<string>();
            
            // Ersletllen der Benötigten Klassen
            
            client = new TcpClient(ui.Domain, ui.Port);

            // Verbinden
            connect(ui,client);

            // Autoflush einstellen

            sw.AutoFlush = true;

            // Wilkommens Nachricht
            sr.ReadLine();

            // User Name Übergeben
            sw.WriteLine("USER " + ui.UserName);

            // leer lesen
            sr.ReadLine();

            // Passwort Senden
            sw.WriteLine("PASS " + ui.Password);

            // leer lesen
            sr.ReadLine();

            // Mail holen
            sw.WriteLine("RETR " + n);

            // Erste line
            tmps = sr.ReadLine();

            // Solange temps nicht "." ist
            while (tmps != ".")
            {
                // tmps hinzufügen
                Nachricht.Add(tmps);
                // neue tmps auslesen
                tmps = sr.ReadLine();
            }



            return Nachricht;

            // Schließen

            sw.WriteLine("QUIT");

            sw.Close();
            sr.Close();

        }
    }
}
