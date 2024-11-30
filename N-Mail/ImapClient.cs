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
        /// Gibt eine Liste der verfügbaren Mailboxen (Ordner) zurück.
        /// </summary>
        /// <returns>Liste der Mailboxen.</returns>
        public List<string> ListMailboxes()
        {
            // Lokale Variablen für die Verarbeitung
            List<string> mailboxes = new List<string>();
            String tmp;

            // Verbindung aufbauen
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
             * Block 2: Mailboxen abfragen
             */
            writer.WriteLine("n" + si + " LIST \"\" \"*\"");
            writer.Flush();
            tmp = reader.ReadLine();

            while (true)
            {
                if (tmp.StartsWith("*"))
                {
                    // Verzeichnisinformationen extrahieren
                    string[] parts = tmp.Split(' ');
                    string mailboxName = parts[parts.Length - 1];
                    mailboxName = mailboxName.Trim('"'); // Entfernen von Anführungszeichen
                    mailboxes.Add(mailboxName);
                }
                else if (tmp.StartsWith("n" + si))
                {
                    break; // Antwort vollständig gelesen
                }

                tmp = reader.ReadLine();
            }

            si++;

            /*
             * Block 3: Verbindung trennen
             */
            Disconnect();

            return mailboxes;
        }

        /// <summary>
        /// Gibt die Anzahl der Mails in einem bestimmten Postfach zurück.
        /// </summary>
        /// <param name="postfach">Das Postfach, dessen Nachrichten gezählt werden sollen.</param>
        /// <returns>Anzahl der Nachrichten im Postfach.</returns>
        public int GetMailCount(string postfach)
        {
            // Lokale Variablen für die Verarbeitung
            int mailCount = 0;
            string tmp;

            // Verbindung aufbauen
            Connect();

            /*
             * Block 1: Einloggen 
             */
            reader.ReadLine(); // Willkommensnachricht
            writer.WriteLine("n" + si + " LOGIN " + ui.UserName + " " + ui.Password);
            writer.Flush();
            reader.ReadLine();
            si++;

            /*
             * Block 2: Postfach auswählen
             */
            writer.WriteLine("n" + si + " SELECT " + postfach);
            writer.Flush();

            while (true)
            {
                tmp = reader.ReadLine();

                // Überprüfen, ob die Zeile die Anzahl der Mails enthält
                if (tmp.StartsWith("*") && tmp.Contains("EXISTS"))
                {
                    // Parse die Anzahl der Nachrichten
                    string[] parts = tmp.Split(' ');
                    mailCount = int.Parse(parts[1]);
                }

                // Ende der SELECT-Antwort erkennen
                if (tmp.StartsWith("n" + si))
                {
                    break;
                }
            }
            si++;

            /*
             * Block 3: Verbindung trennen
             */
            Disconnect();

            return mailCount;
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
            Disconnect();

            return ausgabe;


        }

        /// <summary>
        /// Gibt die Pop3Mail in dem gewählten Postfach als POP3Parser zurück.
        /// </summary>
        /// <param name="postfach">Das Postfach, das der User möchte</param>
        /// <param name="id">Die ID der POP3Parser-Pop3Mail</param>
        /// <returns></returns>
        public String GetMail(String postfach, int id)
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
             *  Block 3: Pop3Mail ausgeben
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
             * Block 4: Pop3Mail-Body ausgeben
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
            Disconnect();

            /*
             * Ausgabe der POP3Parser Pop3Mail 
             */
            return returnStr;

        }

        /// <summary>
        /// Schließen der Verbindung
        /// </summary>
        private void Disconnect()
        {
            reader.Close();
            writer.Close();
        }

    }
}
