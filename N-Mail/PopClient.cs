using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace nMail
{
    public class PopClient
    {
        private System.Net.Sockets.TcpClient client;
        private System.IO.StreamReader sr;
        private System.IO.StreamWriter sw;


        private void initial(UserInformation ui, System.IO.StreamReader sr, System.IO.StreamWriter sw)
        {
            // Wilkommensnachricht
            sr.ReadLine();

            // Username
            sw.WriteLine("USER" + ui.Domain);

            sr.ReadLine();

            // Passwort
            sw.WriteLine("PASS " + ui.Password);
            
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
            Connect(userinformation, client);

            // Autoflush
            sw.AutoFlush = true;

            // Wilkommensnachricht
            WlkMsg = sr.ReadLine();

            // Username übergeben
            sw.WriteLine("USER " + userinformation.UserName);

            sr.ReadLine();

            // Passwort senden
            sw.WriteLine("PASS " + userinformation.Password);
            
            sr.ReadLine();

            // Status
            sw.WriteLine("STAT");

            msg = sr.ReadLine();

            // String splitten
            String[] match = msg.Split(' ');

            // String umwandeln in Integer
            int count = int.Parse(match[1]);

            sw.WriteLine("QUIT");

            // Stream schließen
            sw.Close();
            sr.Close();

            return count;
            
        }
        
        /// <summary>
        /// Stellt eine Verbindung zum Server her.
        /// </summary>
        /// <param name="ui">Benutzerinformation</param>
        /// <param name="reader">Reader</param>
        /// <param name="writer">Writer</param>
        /// <param name="client">TcpClient</param>
        public void Connect(UserInformation ui, System.Net.Sockets.TcpClient client)
        {          
            // Prüft, ob ein SSL-Stream verwendet werden soll.
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
        /// Gibt eine Pop3Mail vom POP3-Server zurück
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<String> getMail(UserInformation ui, int n)
        {
            String tmps;
            List<String> Nachricht = new List<string>();
            
            // Erstellen der benötigten Klassen
            
            client = new TcpClient(ui.Domain, ui.Port);

            // Verbinden
            Connect(ui,client);

            // Autoflush einstellen
            sw.AutoFlush = true;

            // Wilkommensnachricht
            sr.ReadLine();

            // Username Übergeben
            sw.WriteLine("USER " + ui.UserName);

            sr.ReadLine();

            // Passwort senden
            sw.WriteLine("PASS " + ui.Password);

            sr.ReadLine();

            // Pop3Mail holen
            sw.WriteLine("RETR " + n);

            // Erste Zeile auslesen
            tmps = sr.ReadLine();

            // Solange tmps nicht "." ist
            while (tmps != ".")
            {
                // tmps hinzufügen
                Nachricht.Add(tmps);
                // neue tmps auslesen
                tmps = sr.ReadLine();
            }

            // Schließen

            sw.WriteLine("QUIT");

            sw.Close();
            sr.Close();

            return Nachricht;

        }
    }
}
