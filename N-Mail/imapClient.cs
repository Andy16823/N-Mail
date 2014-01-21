using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;


/*
 * Die hier in den methoden und funktionen geschilderten Blocks werden wie folgt verarbeitet
 * 
 * 1.) Dem server einen befehl senden
 * 2.) Den Comand freigeben
 * 3.) Die nachricht des Servers Auslesen
 * 4.) den Session Icncrement wert (si) um eins erhöhen
 *  
 */

namespace nMail
{
    /// <summary>
    /// Stellt einen Clienten für den Entfang von Imap Mails bereit.
    /// </summary>
    public class imapClient
    {
        public System.Net.Sockets.TcpClient client;
        public System.IO.StreamReader reader;
        public System.IO.StreamWriter writer;
        private UserInformation ui;
        private int si;


        /// <summary>
        /// erstellt eine neue imap server klasse
        /// </summary>
        /// <param name="ui"></param>
        public imapClient(UserInformation ui)
        {
            this.ui = ui;
        }

        /// <summary>
        /// verbindet zu dem Angebenen server
        /// </summary>
        public void connect()
        {
            client = new System.Net.Sockets.TcpClient(ui.Domain, ui.Port);

            if(ui.SSL==true)
            {
                // Ssl Stream Erstellen
                SslStream ssl = new SslStream(client.GetStream());
                // Server Authentivizieren
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
          
            // si Wert auf 0 Setzen
            si = 0;
        }
        
        /// <summary>
        /// gibt zu dem Angebenen Postfach die Informationen zurück.
        /// </summary>
        /// <param name="postfach">das postfach aus dem die informationen geladen werden sollen.</param>
        public String malingInfos(String postfach)
        {
            // Conecten
            connect();

            // Deklarieren der Lokalen Varaibln
            String tmp = "";
            String ausgabe = "";

            // entfangen der Wilkommens nachricht
            System.Windows.Forms.MessageBox.Show(reader.ReadLine());

            // Senden des Benutzer Passwortes und der mail Adresse
            writer.WriteLine("n" + si + " LOGIN " + ui.UserName + " " + ui.Password);
            writer.Flush();
            

            // Abfragen der Nachricht
            System.Windows.Forms.MessageBox.Show(reader.ReadLine());
            si++;

            // Senden das wir den Status von unserem Postfach möchten
            writer.WriteLine("n" + si + " select " + postfach);
            writer.Flush();
            

            // Abfragen des Status
            tmp = reader.ReadLine();
            while (true)
            {
                System.Windows.Forms.MessageBox.Show(tmp);
                ausgabe += tmp + "\n";
                tmp = reader.ReadLine();
                if (tmp.StartsWith("n" + si)) break;
            }
            si++;


            // Beenden der Sitzung
            writer.WriteLine("n" + si + " logout");
            writer.Flush();
            si = 0;


            // Disconnect
            disconnect();

            return ausgabe;


        }

        /// <summary>
        /// gibt die Mail in dem gewählten Postfach als mime zurück.
        /// </summary>
        /// <param name="postfach">das Postfach das der user möchtet</param>
        /// <param name="id">die ID der Mime mail</param>
        /// <returns></returns>
        public String getmail(String postfach, int id)
        {
            String tmp;
            String returnStr = "";

            // Conecten
            connect();

            
            /*
             * Block 1 Einlogen 
             */

            reader.ReadLine();
            writer.WriteLine("n" + si + " LOGIN " + ui.UserName + " " + ui.Password);
            writer.Flush();
            reader.ReadLine();
            si++;
                        


            /*
             *  Block 2 Postfach wählen
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
             *  Block 3 Mail Ausgeben
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
             * Block 4 Mail Body Ausgeben
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
             * Block 5 Verbindung und Strams Schließen 
             */


            disconnect();
            reader.Close();
            writer.Close();
            

            /*
             * Ausgabe der Mime Mail 
             */
            

            return returnStr;

        }

        /// <summary>
        /// Schließen der verbindung
        /// </summary>
        public void disconnect()
        {
            reader.Close();
            writer.Close();
        }

    }
}
