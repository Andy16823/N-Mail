using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;


/*
 * Die hier in den Methoden und Funktionen geschilderten Blöcke werden wie folgt verarbeitet
 * 
 * 1.) Dem Server einen Befehl senden
 * 2.) Den Befehl freigeben
 * 3.) Die Nachricht des Servers auslesen
 * 4.) den Session-Increment-Wert (SI) um eins erhöhen
 *  
 */

namespace nMail
{
    /// <summary>
    /// Stellt einen Client für den Empfang von IMAP-Mails bereit
    /// </summary>
    public class ImapClient
    {
        public System.Net.Sockets.TcpClient client;
        public System.IO.StreamReader reader;
        public System.IO.StreamWriter writer;
        private UserInformation ui;
        private int si;


        /// <summary>
        /// Erstellt eine neue IMAP-Client-Klasse
        /// </summary>
        /// <param name="ui"></param>
        public ImapClient(UserInformation ui)
        {
            this.ui = ui;
        }

        /// <summary>
        /// Verbindet zum angebenen Server
        /// </summary>
        public void Connect()
        {
            client = new System.Net.Sockets.TcpClient(ui.Domain, ui.Port);

            if(ui.SSL==true)
            {

                // SSL-Stream erstellen
                SslStream ssl = new SslStream(client.GetStream());
                // Server authentifizieren
                ssl.AuthenticateAsClient(ui.Domain);
                reader = new System.IO.StreamReader(ssl);
                writer = new System.IO.StreamWriter(ssl);

            }
            else if(ui.SSL==false)
            {

                System.IO.Stream str = client.GetStream();
                reader = new System.IO.StreamReader(str);
                writer = new System.IO.StreamWriter(str);

            }
          
            // si-Wert auf 0 setzen
            si = 0;
        }
        
        /// <summary>
        /// Gibt zum angebenen Postfach die Informationen zurück.
        /// </summary>
        /// <param name="postfach">Das Postfach, aus dem die Informationen geladen werden sollen.</param>
        public String MailingInfos(String postfach)
        {
            // Verbinden
            Connect();

            // Deklarieren der lokalen Variablen
            String tmp = "";
            String ausgabe = "";

            // Empfangen der Wilkommensnachricht
            // System.Windows.Forms.MessageBox.Show(reader.ReadLine());

            // Senden des Benutzerpasswortes und der Email-Adresse
            writer.WriteLine("n" + si + " LOGIN " + ui.UserName + " " + ui.Password);
            writer.Flush();
            

            // Abfragen der Nachricht
            //System.Windows.Forms.MessageBox.Show(reader.ReadLine());

            si++;

            // Senden, das wir den Status von unserem Postfach möchten
            writer.WriteLine("n" + si + " select " + postfach);
            writer.Flush();
            

            // Abfragen des Status
            tmp = reader.ReadLine();
            while (true)
            {
                // System.Windows.Forms.MessageBox.Show(tmp);
                ausgabe += tmp + "\n";
                tmp = reader.ReadLine();
                if (tmp.StartsWith("n" + si)) break;
            }
            si++;


            // Beenden der Sitzung
            writer.WriteLine("n" + si + " logout");
            writer.Flush();
            si = 0;


            // Verbindung trennen
            disconnect();

            return ausgabe;


        }

        /// <summary>
        /// Gibt die Mail in dem gewählten Postfach als Mime zurück.
        /// </summary>
        /// <param name="postfach">Das Postfach, das der User möchte</param>
        /// <param name="id">Die ID der Mime-Mail</param>
        /// <returns></returns>
        public String getMail(String postfach, int id)
        {
            String tmp;
            String returnStr = "";

            // Conecten
            Connect();

            
            /*
             * Block 1: Einloggen 
             */

            reader.ReadLine();
            writer.WriteLine("n" + si + " LOGIN " + ui.UserName + " " + ui.Password);
            writer.Flush();
            reader.ReadLine();
            si++;
                        


            /*
             *  Block 2: Postfach wählen
             */


            writer.WriteLine("n" + si + " select " + postfach);
            writer.Flush();
            tmp = reader.ReadLine();
            while (true)
            {
                tmp = reader.ReadLine();
                if (tmp.StartsWith("n" + si)) break;
            }
            si++;



            /*
             *  Block 3: Mail ausgeben
             */


            writer.WriteLine("n" + si + " fetch " + id + " body[HEADER]");
            writer.Flush();
            tmp = reader.ReadLine();
            while (true)
            {
                returnStr += tmp + "\n";
                tmp = reader.ReadLine();
                if (tmp.StartsWith("n" + si)) break;

            }
            si++;


            /*
             * Block 4: Mail-Body ausgeben
             */


            writer.WriteLine("n" + si + " fetch " + id + " body[TEXT]");
            writer.Flush();
            tmp = reader.ReadLine();
            while (true)
            {
                returnStr += tmp + "\n";
                tmp = reader.ReadLine();
                if (tmp.StartsWith("n" + si)) break;
            }
            si++;


            /*
             * Block 5: Verbindung und Strams Schließen 
             */


            disconnect();

            /*
             * Ausgabe der Mime Mail 
             */
            

            return returnStr;

        }

        /// <summary>
        /// Schließen der Verbindung
        /// </summary>
        private void disconnect()
        {
            reader.Close();
            writer.Close();
        }

    }
}
