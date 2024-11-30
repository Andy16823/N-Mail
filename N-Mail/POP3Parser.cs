using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace nMail
{
    public class POP3Parser
    {

        /// <summary>
        /// Prüft, ob der Contenttyp ein MixedMultipart ist
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public Boolean IsMixedMultipart(List<String> mime)
        {
            Boolean isMultipart = false;

            foreach (String item in mime)
            {
                if (item.Contains("Content-Type: multipart/alternative;"))
                {
                    isMultipart = true;
                }
            }

            return isMultipart;

        }
        
        /// <summary>
        /// Gibt den Contenttyp der Pop3Mail zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String GetContentType(List<String> mime)
        {
            String ContentType = "";

            foreach (String item in mime)
            {
                if (item.Contains("Content-Type:"))
                {
                    Match match = Regex.Match(item, ".*\\:(?<content>.*)");
                    ContentType = match.Groups["content"].Value;
                    break;
                }
            }
            
            return ContentType;
        }
        
        /// <summary>
        /// Prüft, ob die Pop3Mail vom Typ Multipart ist
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public Boolean IsMultipart(List<String> mime)
        {
            Boolean isMultipart = false;

            foreach (String item in mime)
            {
                if (item.Contains("Content-Type: multipart/alternative;"))
                {
                    isMultipart = true;
                }
            }

            return isMultipart;
        }

        /// <summary>
        /// Prüft ob die Pop3Mail vom Typ Plain ist
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public Boolean HasPlainText(List<String> mime)
        {
            Boolean hasPlain = false;

            foreach (String item in mime)
            {
                if (item.Contains("Content-Type: text/plain;"))
                {
                    hasPlain = true;
                }
            }

            return hasPlain;

        }

        /// <summary>
        /// Mit dieser Funktion kann geprüft werden, ob das Dokument HTML-Content besitzt.
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public Boolean HasHtmlText(List<String> mime)
        {

            Boolean hasHtml = false;

            foreach (String item in mime)
            {
                if (item.Contains("Content-Type: text/html;"))
                {
                    hasHtml = true;
                }
            }

            return hasHtml;

        }

        /// <summary>
        /// Gibt den Body der Pop3Mail als text/plain zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        [Obsolete("Bitte nutzen Sie die Funktion BodyPlain")]
        public String GetBodyAsPlainText(List<String> mime)
        {

            String keyword = "boundary";
            String boundary = "";
            String bdy = "";
            int z = 0;
            int Start = 0;
            int Ende = 0;

            // nach dem Boundary suchen

            foreach (String item in mime)
            {
                
                if(item.Contains(keyword))
                {
                    String[] matches = item.Split('"');
                    boundary = matches[1];
                }


            }



            // nach dem Contenttyp suchen
            foreach (String item in mime)
            {

                if (item.Contains("Content-Type: text/plain;"))
                {
                    Start = z + 2;
                    break;
                }

                z++;

            }

            // nach dem Ende suchen
            for (int i = Start; i < mime.Count; i++)
            {
                if (mime[i].Contains(boundary))
                {
                    Ende = i;
                    break;
                }
            }



            // Prüfen, ob es einen Start und Endewert gibt.
            if (Start != 0 && Ende != 0)
            {
                // Text zwischen Start und Ende ausgeben
                for (int i = Start; i < Ende; i++)
                {
                    bdy += mime[i] + "\n";
                
                }
            }

            return bdy;

        }

        /// <summary>
        /// Gibt den Body der Pop3Mail als HTML-Text zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        [Obsolete("Bitte nutzen Sie die Funktion BodyHtml")]
        public String GetBodyAsHtmlText(List<String> mime)
        {

            String keyword = "boundary";
            String boundary = "";
            String bdy = "";
            int z = 0;
            int Start = 0;
            int Ende = 0;

            // nach dem Boundary Suchen

            foreach (String item in mime)
            {

                if (item.Contains(keyword))
                {
                    String[] matches = item.Split('"');
                    boundary = matches[1];
                }


            }



            // nach dem Content suchen
            foreach (String item in mime)
            {

                if (item.Contains("Content-Type: text/html;"))
                {
                    Start = z + 2;
                    break;
                }

                z++;

            }


            // nach dem Ende suchen
            for (int i = Start; i < mime.Count; i++)
            {
                if (mime[i].Contains(boundary))
                {
                    Ende = i;
                    break;
                }
            }



            // Prüfen, ob es einen Start und Ende Wert giebt.
            if (Start != 0 && Ende != 0)
            {
                // Text zwischen Start und Ende ausgeben
                for (int i = Start; i < Ende; i++)
                {
                    bdy += mime[i] + "\n";

                }
            }

            return bdy;


        }


        /// <summary>
        /// Gibt einen normalen Text ohne Multipart zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String GetNoneMultipart(List<String> mime)
        {
            String bdy = "";
            int Start = 0;
            int z = 0;

            foreach (String item in mime)
            {
                if (String.IsNullOrEmpty(item))
                {
                    Start = z;
                    break;
                }
                z++;
            }


            if (Start != 0)
            {
                for (int i = Start; i < mime.Count; i++)
                {
                    bdy += mime[i] + "\n";
                }
            }

            return bdy;
            
        }

        /// <summary>
        /// Gibt den Betreff zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String GetSubject(List<String> mime)
        {
            String subject = "";
            String[] match = null;
            foreach (String item in mime)
            {
                if (item.Contains("Subject"))
                {
                    match = item.Split(':');
                    subject = match[1];                  
                    break;
                }
            }

            return subject;

        }


        /// <summary>
        /// Gibt die Absender-Adresse zurück.
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String GetSender(List<String> mime)
        {
            String sender="";

            foreach (String item in mime)
            {

                if (item.Contains("From:"))
                {
                    Match match = Regex.Match(item, ".*<(?<mailadresse>.*)>.*");
                    sender = match.Groups["mailadresse"].Value;
                }

            }



            return sender;
        }

        /// <summary>
        /// Gibt den Empfänger der Pop3Mail zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String GetTo(List<String> mime)
        {
            String to = "";

            foreach (String item in mime)
            {
                if (item.Contains("Envelope-to:"))
                {
                    Match match = Regex.Match(item, ".*\\:(?<empfänger>.*)");
                    to = match.Groups["empfänger"].Value;
                    break;
                }

                
            }

            return to;
        }

        /// <summary>
        /// Gibt das Empfangsdatum der Pop3Mail zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String GetDeliveryDate(List<String> mime)
        {
            String deliverydate = "";

            foreach (String item in mime)
            {
                if (item.Contains("Delivery-date:"))
                {

                    Match match = Regex.Match(item, ".*\\:(?<date>)");
                    deliverydate = match.Groups["date"].Value;
                    break;

                }
            }
            
            return deliverydate;
            
        }

        /// <summary>
        /// Gibt das Sendedatum der Pop3Mail zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String GetSendDate(List<String> mime)
        {
            String senddate = "";

            foreach (String item in mime)
            {

                if (item.Contains("Date:"))
                {
                    Match match = Regex.Match(item, ".*\\:(?<date>.*)");
                    senddate = match.Groups["date"].Value;
                    break;
                }

            }
            
            return senddate;
        }

        /// <summary>
        /// Gibt die Anzahl der Dateianhänge zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public int GetAttachmentCount(List<String> mime)
        {
            int count = 0;

            foreach (String item in mime)
            {
                if (item.Contains("Content-Disposition: attachment;"))
                {
                    count++;
                }
            }

            return count;

        }

        /// <summary>
        /// Gibt eine Liste der Dateianhänge zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public List<Attachment> GetAttachments(List<String> mime, String boundary)
        {
            int attachCount = GetAttachmentCount(mime);
            List<Attachment> attach = new List<Attachment>();
            
            int z = 0;
            int start = 0;
            int ende = 0;
            int fnameLine = 0;
            int encodeline = 0;

            foreach (String item in mime)
            {

                if (item.Contains("Content-Disposition: attachment;"))
                {
                    Attachment att = new Attachment();

                    // Startzeile suchen
                    for (int i = z; i < mime.Count; i++)
                    {
                        if (String.IsNullOrEmpty(mime[i]))
                        {
                            start = i + 1;
                            break;
                        }

                    }

                    // Endzeile suchen
                    for (int i = start; i < mime.Count; i++)
                    {
                        if (mime[i].Contains(boundary))
                        {
                            ende = i;
                            break;
                        }
                    }

                    // Suchen der Zeile mit dem Namen
                    for (int i = z; i < ende; i++)
                    {
                        if (mime[i].Contains("filename="))
                        {
                            fnameLine = i;
                            break;
                        }
                    }

                    // Suchen der Zeile mit dem Encode-Typ
                    for (int i = z; i < ende; i++)
                    {
                        if (mime[i].Contains("Content-Transfer-Encoding:"))
                        {
                            encodeline = i;
                            break;
                        }
                    }

                    // Filename-Pattern
                    Match filenameMatch = Regex.Match(mime[fnameLine], ".*\\=(?<inner>.*)");
                    att.Name = filenameMatch.Groups["inner"].Value;

                    // Encode-Typ-Pattern
                    Match encodematch = Regex.Match(mime[encodeline], ".*\\:(?<inner>.*)");
                    att.EncodeBase = encodematch.Groups["inner"].Value;

                    // Bytes auslesen
                    String byteString = "";
                    for (int i = start; i < ende; i++)
                    {
                        byteString = byteString + mime[i];
                    }

                    Byte[] attachbytes = Convert.FromBase64String(byteString);
                    att.Bytes = attachbytes;

                    // Attachment der Anhangsliste übergeben
                    attach.Add(att);
                }
                z++;
            }
            return attach;
        }

        /// <summary>
        /// Gibt die Pop3Mail im POP3Parser-Format zurück. Sollte ein Fehler auftreten, bitte diese Funktion ausführen."
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String GetMimeMail(List<String> mime)
        {
            String mimestr="";

            foreach (String item in mime)
            {
                mimestr = mimestr + item;
            }

            return mimestr;
        }

        /// <summary>
        /// Gibt das Boundary für die Anlagen zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String GetBoundary(List<String> mime)
        {
            String boundary = "";

            foreach (String item in mime)
            {
                if (item.Contains("boundary="))
                {
                    Match match = Regex.Match(item, ".*\\=(?<inner>.*)");
                    String boundaryUnformatted = match.Groups["inner"].Value;
                    if (boundaryUnformatted.Contains('"'))
                    {
                        Char[] chr = new Char[] {'\"'};
                        boundary = boundaryUnformatted.Replace(chr[0].ToString(),String.Empty);
                    }
                    else
                    {
                        boundary = boundaryUnformatted;
                    }
                }
            }

            return boundary;

        }

        /// <summary>
        /// Gibt den Html-Body aus.
        /// </summary>
        /// <param name="mime"></param>
        /// <param name="boundary"></param>
        /// <returns></returns>
        public String HtmlBody(List<String> mime, String boundary)
        {
            String body = "";
            int z = 0;
            int Start = 0;
            int ende = 0;
            

            foreach (String item in mime)
            {
                if (item.Contains("Content-Type: text/html;"))
                {
                    // Ab der gefundenen Zeile durchlaufen, bis der Content beginnt.
                    for (int i = z; i < mime.Count; i++)
                    {
                        // Wenn eine leere Zeile vorkommt, Startzeile festlegen
                        if (String.IsNullOrEmpty(mime[i]))
                        {
                            Start = i;
                            break;
                        }
                    }

                    // Ab der Startzeile durchlaufen, bis das Boundary kommt
                    for (int i = Start; i < mime.Count; i++)
                    {
                        // Wenn die Zeile das Boundary beinhaltet, Endzeile bestimmen
                        if (mime[i].Contains(boundary))
                        {
                            ende = i;
                            break;
                        }
                    }

                    // Wenn fertig, dann unterbrechen
                    break;
                    
                }

                // Wert erhöhen
                z++;
                    
            }

            // Body ausgeben

            for (int i = Start; i < ende; i++)
            {
                body = body + mime[i];
            }

            return body;

        }

        /// <summary>
        /// Gibt den Body als text/plain zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <param name="boundary"></param>
        /// <returns></returns>
        public String PlainBody(List<String> mime, String boundary)
        {
            String Plain="";
            int z = 0;
            int Start = 0;
            int ende = 0;

            foreach (String item in mime)
            {

                if(item.Contains("Content-Type: text/plain;"))
                {
                    // Startzeile holen
                    for(int i = z; i < mime.Count ; i++)
                    {
                        if(String.IsNullOrEmpty(mime[i]))
                        {
                            Start = i;
                            break;
                        }
                    }

                    // Endzeile holen
                    for(int i = Start ; i < mime.Count ; i++)
                    {
                        if(mime[i].Contains(boundary))
                        {
                            ende = i;
                            break;
                        }
                    }

                    // Schleife beenden
                    break;
                }

                z++;

            }

            // Ausgeben
            for (int i = Start; i < ende; i++)
            {
                Plain = Plain + mime[i];
            }

            return Plain;

        }
    
    }
}
