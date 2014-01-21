using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/*
 * Dieser Mime Parser ist nur für mails die über den Imap Clienten Abgerufen wurde, 
 * für pop3 Mails benutzen Sie bitte die Klasse mime
 */

namespace nMail
{
    public class imapMime
    {

        public List<String> mime;

        /*
         * Text Ausgaben 
         */


        /// <summary>
        /// Initailisiert eine Neue klasse des Imap Mime
        /// </summary>
        /// <param name="mimeMail"></param>
        public imapMime(String mimeMail)
        {
            mime = new List<string>();
            String[] match = mimeMail.Split('\n');
            foreach (String item in match)
            {
                mime.Add(item);
            }
        }

        /// <summary>
        /// gibt das boundary zurück das die mail aufteilt
        /// </summary>
        /// <returns></returns>
        public String boundarray()
        {
            String ausgabe = "";

            foreach (String item in mime)
            {
                if (item.Contains("boundary"))
                {
                    String[] match = item.Split('=');
                    String barrUnformattet = match[1];
                    ausgabe = barrUnformattet.Replace("\"", "");
                }
            }

            return ausgabe;
        }

        /// <summary>
        /// gibt den body als html format zurück
        /// </summary>
        /// <param name="brr">das Boundaray</param>
        /// <returns></returns>
        public String bodyAsHtml(String brr)
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
                    // Start Contente holen
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

            // Content von start nach ende durchlaufen
            for(int i = Start ; i < Ende ; i++)
            {
                ausgabe += mime[i] + "\n";
            }

            return ausgabe;
        }

        /// <summary>
        /// gibt den body als Plain Text format zurück
        /// </summary>
        /// <param name="brr"></param>
        /// <returns></returns>
        public String bodyAsPlain(String brr)
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
                    // Start Content Suchen
                    for (int i = z; i > 0; i--)
                    {
                        if (mime[i].Contains(brr))
                        {
                            cStart = i;
                            break;
                        }
                    }

                    // Start des Html Bereiches
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

                    // beenden der Schleife
                    break;
                }

                z++;
            }

            // Ausgeben des Plain Bereiches
            for (int i = Start; i < Ende; i++)
            {
                ausgabe += mime[i] + "\n";
            }


            return ausgabe;
        }

        /// <summary>
        /// gibt den body als none Multipart zurück
        /// </summary>
        /// <returns></returns>
        public String noneMultiPart()
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

            // Ausgeben der Mail

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
        /// gibt den sender der mail zurück
        /// </summary>
        /// <returns></returns>
        public String sender()
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
        /// gibt den Betreff der E-mail zurück
        /// </summary>
        /// <returns></returns>
        public String Subject()
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
        /// gibt das empfangsdatum der email zurück
        /// </summary>
        /// <returns></returns>
        public String DeliveryDate()
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
    
        
        /*
         * Attachments Ausgeben
         */


        /// <summary>
        /// gibt die Datei Anhänge der Mail zurück
        /// </summary>
        /// <param name="brr"></param>
        /// <returns></returns>
        public List<Attachment> Attachments(String brr)
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

                    // Start des Contents Suchen
                    for ( int i = z ; i > 0 ; i--)
                    {
                        if(mime[i].Contains(brr))
                        {
                            cStart = i;
                            break;
                        }
                    }

                    // Start des Byte Strings suchen
                    for(int i = cStart ; i < mime.Count ; i++)
                    {
                        if(String.IsNullOrEmpty(mime[i]))
                        {
                            Start = i;
                            break;
                        }
                    }

                    // Ende des Contents Suchen
                    for(int i = Start ; i < mime.Count ; i++)
                    {
                        if(mime[i].Contains(brr))
                        {
                            Ende = i;
                            break;
                        }
                    }

                    // Ausgbabe des Content Types
                    for(int i = cStart; i < Start; i++)
                    {
                        if(mime[i].Contains("Content-Type:"))
                        {
                            att.ContentTyp = mime[i].Replace("Content-Type:", "");
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
                            att.Encodebase = match.Groups["inner"].Value;
                            break;
                        }
                    }

                    // Ausgabe und umandeln des Strings
                    String byteStr = "";
                    for(int i = Start ; i < Ende ; i++)
                    {
                        byteStr += mime[i];
                    }
                    att.bytes = Convert.FromBase64String(byteStr);

                    // Attachmant hinzufügen
                    attachts.Add(att);

                }
                z++;
            }

            return attachts;

        }
    
        /// <summary>
        /// gibt die Anzahl der Datei anhänge zurück
        /// </summary>
        /// <returns></returns>
        public int AttachmentCount()
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
		 

        /*
         * Boolenische Werte
         */

        /// <summary>
        /// gibt einen boolenischen wert zurück ob die mime mail das format hasHtmlBody beinhaltet
        /// </summary>
        /// <returns></returns>
        public Boolean hasHtmlBody()
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
        /// gibt einen booleinischen wert zurück pb die mime mail das format Plain-Text beinhaltet
        /// </summary>
        /// <returns></returns>
        public Boolean hasPlainBody()
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
        /// gibt einen boolenischen wert zurück ob die mime mail einen Multipart hat
        /// </summary>
        /// <returns></returns>
        public Boolean hasMultipart()
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
        /// gibt einen boolenischen wert zurück ob die mime mail einen Mixed Multipart beinhaltet
        /// </summary>
        /// <returns></returns>
        public Boolean hasMixedMultipart()
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
        /// Prüft ob ein Bound Array Vorhanden ist.
        /// </summary>
        /// <returns></returns>
        public Boolean hasBoundaray()
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

        /// <summary>
        /// Prüft ob es Datei Anhänge in der Mail gibt
        /// </summary>
        /// <returns></returns>
        public Boolean hasAttachmants()
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
