using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/*
 * Dieser POP3Parser-Parser ist nur für Mails, die über den Imap-Client abgerufen wurden, 
 * Für POP3-Mails benutzen Sie bitte die Klasse POP3Parser
 */

namespace nMail
{
    public class ImapParser
    {

        public List<String> mime;

        /// <summary>
        /// Initialisiert die Klasse ImapParser
        /// </summary>
        /// <param name="mimeMail"></param>
        public ImapParser(String mimeMail)
        {
            mime = new List<string>();
            String[] match = mimeMail.Split('\n');
            foreach (String item in match)
            {
                mime.Add(item);
            }
        }

        /// <summary>
        /// Gibt das Boundary zurück, das die mail aufteilt
        /// </summary>
        /// <returns></returns>
        public String GetBoundary()
        {
            String ausgabe = "";

            foreach (String item in mime)
            {
                if (item.Contains("boundary"))
                {
                    String[] match = item.Split('=');
                    String barrUnformatted = match[1];
                    ausgabe = barrUnformatted.Replace("\"", "");
                }
            }

            return ausgabe;
        }

        /// <summary>
        /// Gibt den Body im HTML-Format zurück
        /// </summary>
        /// <param name="brr">das Boundary</param>
        /// <returns></returns>
        public String GetBodyAsHtml(String brr)
        {
            String ausgabe = "";
            int z = 0;
            int cStart = 0;
            int Start = 0;
            int Ende = 0;

            foreach (String item in mime)
            {
                if(item.Contains("Content-Type: text/html;"))
                {
                    // Start-Content holen
                    for(int i = z ; i > 0 ; i--)
                    {
                        if(mime[i].Contains(brr))
                        {
                            cStart = i;
                            break;
                        }
                    }

                    // Start des Textes holen
                    for(int i = cStart ; i < mime.Count ; i++)
                    {
                        if(String.IsNullOrEmpty(mime[i]))
                        {
                            Start = i;
                            break;
                        }
                    }

                    // Ende des Content
                    for(int i = Start; i < mime.Count ; i++)
                    {
                        if(mime[i].Contains(brr))
                        {
                            Ende = i;
                            break;
                        }
                    }

                    // Schleife beeenden
                    break;
                }

                z++;
            }

            // Content von Start bis Ende durchlaufen
            for(int i = Start ; i < Ende ; i++)
            {
                ausgabe += mime[i] + "\n";
            }

            return ausgabe;
        }

        /// <summary>
        /// Gibt den Body als Plain-Text zurück
        /// </summary>
        /// <param name="brr">Das Boundary</param>
        /// <returns></returns>
        public String GetBodyAsPlain(String brr)
        {
            String ausgabe = "";
            int z = 0;
            int cStart = 0;
            int Start = 0;
            int Ende = 0;

            foreach (String item in mime)
            {
                if (item.Contains("Content-Type: text/plain;"))
                {
                    // Start-Content Suchen
                    for (int i = z; i > 0; i--)
                    {
                        if (mime[i].Contains(brr))
                        {
                            cStart = i;
                            break;
                        }
                    }

                    // Start des HTML-Bereiches
                    for (int i = cStart; i < mime.Count; i++)
                    {
                        if (String.IsNullOrEmpty(mime[i]))
                        {
                            Start = i;
                            break;
                        }
                    }

                    // Ende des Content
                    for (int i = Start; i < mime.Count; i++)
                    {
                        if (mime[i].Contains(brr))
                        {
                            Ende = i;
                            break;
                        }
                    }

                    // Beenden der Schleife
                    break;
                }

                z++;
            }

            // Ausgeben des Plain-Content
            for (int i = Start; i < Ende; i++)
            {
                ausgabe += mime[i] + "\n";
            }


            return ausgabe;
        }

        /// <summary>
        /// Gibt den Body als NoneMultipart zurück
        /// </summary>
        /// <returns></returns>
        public String GetTextAsNoneMultiPart()
        {
            String ausgabe = "";
            int Start = 0;
            int z = 0;

            // Suchen des Beginns

            foreach (String item in mime)
            {
                if (String.IsNullOrEmpty(item))
                {
                    Start = z;
                    break;
                }
                z++;
            }

            // Ausgeben der Pop3Mail

            for (int i = Start; i < mime.Count; i++)
            {
                if (!mime[i].StartsWith(")") && !mime[i].StartsWith("*"))
                {
                    ausgabe += mime[i] + "\n";
                }
            }
            return ausgabe;
        }

        /// <summary>
        /// Gibt den Sender der Pop3Mail zurück
        /// </summary>
        /// <returns></returns>
        public String GetReturnPath()
        {
            String ausgabe = "";

            foreach (String item in mime)
            {
                if (item.StartsWith("Return-path"))
                {
                    Match match = Regex.Match(item, ".*\\<(?<inner>.*)\\>");
                    ausgabe = match.Groups["inner"].Value;
                    break;
                }
            }

            return ausgabe;
        }

        /// <summary>
        /// Gibt den Betreff der Email zurück
        /// </summary>
        /// <returns></returns>
        public String GetSubject()
        {
            String ausgabe = "";

            foreach (String item in mime)
            {
                if (item.StartsWith("Subject"))
                {
                    ausgabe = item.Replace("Subject:", "");
                    break;
                }
            }

            return ausgabe;
        }

        /// <summary>
        /// Gibt das Empfangsdatum der Email zurück
        /// </summary>
        /// <returns></returns>
        public String GetDeliveryDate()
        {
            String ausgabe = "";

            foreach (String item in mime)
            {
                if (item.StartsWith("Delivery-date"))
                {
                    ausgabe = item.Replace("Delivery-date:", "");
                    break;
                }
            }
            
            return ausgabe;
        }

        /// <summary>
        /// Gibt die Dateianhänge der Pop3Mail zurück
        /// </summary>
        /// <param name="brr"></param>
        /// <returns></returns>
        public List<Attachment> GetAttachments(String brr)
        {
            List<Attachment> attachts = new List<Attachment>();

            int z = 0;
            int cStart=0;
            int Start = 0;
            int Ende = 0;


            foreach (String item in mime)
            {
                if(item.Contains("Content-Disposition: attachment;"))
                {

                    Attachment att = new Attachment();

                    // Start des Contents suchen
                    for ( int i = z ; i > 0 ; i--)
                    {
                        if(mime[i].Contains(brr))
                        {
                            cStart = i;
                            break;
                        }
                    }

                    // Start des Byte-Strings suchen
                    for(int i = cStart ; i < mime.Count ; i++)
                    {
                        if(String.IsNullOrEmpty(mime[i]))
                        {
                            Start = i;
                            break;
                        }
                    }

                    // Ende des Contents suchen
                    for(int i = Start ; i < mime.Count ; i++)
                    {
                        if(mime[i].Contains(brr))
                        {
                            Ende = i;
                            break;
                        }
                    }

                    // Ausgabe des Contenttyps
                    for(int i = cStart; i < Start; i++)
                    {
                        if(mime[i].Contains("Content-Type:"))
                        {
                            att.ContentType = mime[i].Replace("Content-Type:", "");
                            break;
                        }
                    }

                    // Ausgabe des Namen
                    for(int i = cStart ; i < Start ; i++)
                    {
                        if(mime[i].Contains("filename"))
                        {
                            Match match = Regex.Match(mime[i],".*\\=(?<inner>.*)");
                            att.Name = match.Groups["inner"].Value.Replace("\"","");
                            break;
                        }
                    }

                    // Ausgabe der Encodebase
                    for(int i = cStart; i < Start ; i++)
                    {
                        if(mime[i].Contains("Content-Transfer-Encoding"))
                        {
                            Match match = Regex.Match(mime[i],".*\\:(?<inner>.*)");
                            att.EncodeBase = match.Groups["inner"].Value;
                            break;
                        }
                    }

                    // Ausgabe und Umwandeln des Strings
                    String byteStr = "";
                    for(int i = Start ; i < Ende ; i++)
                    {
                        byteStr += mime[i];
                    }
                    att.Bytes = Convert.FromBase64String(byteStr);

                    // Attachment hinzufügen
                    attachts.Add(att);

                }
                z++;
            }

            return attachts;

        }
    
        /// <summary>
        /// Gibt die Anzahl der Dateianhänge zurück
        /// </summary>
        /// <returns></returns>
        public int GetAttachmentCount()
        {
            int ausgabe = 0;

            foreach (String item in mime)
            {
                if(item.Contains("Content-Disposition: attachment;"))
                {
                    ausgabe ++;
                }
            }

            return ausgabe;
        }

        /// <summary>
        /// prüft, ob die POP3Parser-Pop3Mail einen HTML-Body beinhaltet
        /// </summary>
        /// <returns></returns>
        public Boolean HasHtmlBody()
        {
            Boolean ausgabe = false;

            foreach (String item in mime)
            {
                if(item.Contains("Content-Type: text/html;"))
                {
                    ausgabe = true;
                    break;
                }
            }

            return ausgabe;
        }

        /// <summary>
        /// Prüft, ob die POP3Parser-Pop3Mail einen Plain-Body beinhaltet
        /// </summary>
        /// <returns></returns>
        public Boolean HasPlainBody()
        {
            Boolean ausgabe = false;

            foreach (String item in mime)
            {
                if (item.Contains("Content-Type: text/plain;"))
                {
                    ausgabe = true;
                    break;
                }
            }

            return ausgabe;

        }

        /// <summary>
        /// Prüft, ob die POP3Parser-Pop3Mail einen Multipart hat
        /// </summary>
        /// <returns></returns>
        public Boolean HasMultipart()
        {
            Boolean ausgabe = false;

            foreach (String item in mime)
            {
                if(item.Contains("multipart/alternative"))
                {
                    ausgabe = true;
                    break;
                }
            }

            return ausgabe;
        }

        /// <summary>
        /// Prüft, ob die POP3Parser-Pop3Mail einen MixedMultipart beinhaltet
        /// </summary>
        /// <returns></returns>
        public Boolean HasMixedMultipart()
        {
            Boolean ausgabe = false;

            foreach (String item in mime)
            {
                if (item.Contains("multipart/mixed"))
                {
                    ausgabe = true;
                    break;
                }
            }

            return ausgabe;
        }

        /// <summary>
        /// Prüft ob ein Boundary Vorhanden ist.
        /// </summary>
        /// <returns></returns>
        public Boolean HasBoundary()
        {
            Boolean ausgabe = false;

            foreach (String item in mime)
            {
                if (item.Contains("boundary"))
                {
                    ausgabe = true;
                    break;
                }
            }

            return ausgabe;

        }

        public String GetSender()
        {
            foreach (String item in mime)
            {
                if(item.Contains("X-Envelope-From:"))
                {
                    var match = Regex.Match(item, @"<([^>]*)>");
                    return match.Groups[1].Value;
                }
            }
            return "";
        }

        /// <summary>
        /// Prüft, ob es Dateianhänge in der Pop3Mail gibt
        /// </summary>
        /// <returns></returns>
        public Boolean HasAttachments()
        {
            Boolean ausgabe = false;

            foreach (String item in mime)
            {
                if(item.Contains("Content-Disposition: attachment;"))
                {
                    ausgabe = true;
                    break;
                }
            }

            return ausgabe;
        }
    }
}
