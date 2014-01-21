using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace nMail
{
    public class mime
    {

        /// <summary>
        /// Prüft ob der Content Typ ein Mixed Multipart ist
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public Boolean isMixedMultipart(List<String> mime)
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
        /// Gibt den Content Typ der Mail zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String getContetntTyp(List<String> mime)
        {
            String ContentTyp = "";

            foreach (String item in mime)
            {
                if (item.Contains("Content-Type:"))
                {
                    Match match = Regex.Match(item, ".*\\:(?<content>.*)");
                    ContentTyp = match.Groups["content"].Value;
                    break;
                }
            }
            
            return ContentTyp;
        }
        
        /// <summary>
        /// prüft ob es in der mail einen Content-Type: multipart giebt
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public Boolean isMultipart(List<String> mime)
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
        /// Prüft ob es in der Mail einen Plain Content giebt
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public Boolean hasPlainText(List<String> mime)
        {
            Boolean hasPlain = false;
            String mail;

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
        /// mit dieser funktion kann geprüft werden ob das Dokument einen html Content besitzt.
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public Boolean hasHtmlText(List<String> mime)
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
        /// giebt den body der mail als text/plain zurück (Veraltet nicht Benutzen !! bitte nutzen Sie BodyPlain)
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String getBodyasPlaynText(List<String> mime)
        {

            String keyword = "boundary";
            String boundary = "";
            String bdy = "";
            int z = 0;
            int Start = 0;
            int Ende = 0;

            // nach dem Bound Array Suchen

            foreach (String item in mime)
            {
                
                if(item.Contains(keyword))
                {
                    String[] matches = item.Split('"');
                    boundary = matches[1];
                }


            }



            // nach dem Content Suchen
            foreach (String item in mime)
            {

                if (item.Contains("Content-Type: text/plain;"))
                {
                    Start = z + 2;
                    break;
                }

                z++;

            }


            // nach dem Ende Suchen
            for (int i = Start; i < mime.Count; i++)
            {
                if (mime[i].Contains(boundary))
                {
                    Ende = i;
                    break;
                }
            }



            // Prüfen ob es einen Start und Ende Wert giebt.
            if (Start != 0 && Ende != 0)
            {
                // Text zwichen Start und Ende Ausgeben
                for (int i = Start; i < Ende; i++)
                {
                    bdy += mime[i] + "\n";
                
                }
            }

            return bdy;


        }

        /// <summary>
        /// giebt den body der mail als html version zurück (Veraltet nicht Benutzen !! bitte nutzen Sie BodyHtml)
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String getBodyasHtmlText(List<String> mime)
        {

            String keyword = "boundary";
            String boundary = "";
            String bdy = "";
            int z = 0;
            int Start = 0;
            int Ende = 0;

            // nach dem Bound Array Suchen

            foreach (String item in mime)
            {

                if (item.Contains(keyword))
                {
                    String[] matches = item.Split('"');
                    boundary = matches[1];
                }


            }



            // nach dem Content Suchen
            foreach (String item in mime)
            {

                if (item.Contains("Content-Type: text/html;"))
                {
                    Start = z + 2;
                    break;
                }

                z++;

            }


            // nach dem Ende Suchen
            for (int i = Start; i < mime.Count; i++)
            {
                if (mime[i].Contains(boundary))
                {
                    Ende = i;
                    break;
                }
            }



            // Prüfen ob es einen Start und Ende Wert giebt.
            if (Start != 0 && Ende != 0)
            {
                // Text zwichen Start und Ende Ausgeben
                for (int i = Start; i < Ende; i++)
                {
                    bdy += mime[i] + "\n";

                }
            }

            return bdy;


        }


        /// <summary>
        /// Giebt einen normalen Text ohne Multipart zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String getNoneMultipart(List<String> mime)
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
        /// giebt den Betreff zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String getSubject(List<String> mime)
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
        /// giebt die absende addresse heraus.
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String getSender(List<String> mime)
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
        /// giebt den Empfänger der Mail zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String getTo(List<String> mime)
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
        /// giebt das empfangsdatum der mail zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String getDeliverydate(List<String> mime)
        {
            String Deliverydate = "";

            foreach (String item in mime)
            {
                if (item.Contains("Delivery-date:"))
                {

                    Match match = Regex.Match(item, ".*\\:(?<date>)");
                    Deliverydate = match.Groups["date"].Value;
                    break;

                }
            }
            
            return Deliverydate;
            
        }

        /// <summary>
        /// giebt das sende datum der mail zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String getSendDate(List<String> mime)
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
        /// giebt die Anzahl der dateianhänge zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public int getAttachmantCount(List<String> mime)
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
        public List<Attachment> getAttachmants(List<String> mime, String boundarray)
        {
            int attachCount = getAttachmantCount(mime);
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

                    // Start Zeile suchen
                    for (int i = z; i < mime.Count; i++)
                    {
                        if (String.IsNullOrEmpty(mime[i]))
                        {
                            start = i + 1;
                            break;
                        }

                    }

                    // End Zeile Suchen
                    for (int i = start; i < mime.Count; i++)
                    {
                        if (mime[i].Contains(boundarray))
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

                    // Suchen der Zeile mit dem encode typ
                    for (int i = z; i < ende; i++)
                    {
                        if (mime[i].Contains("Content-Transfer-Encoding:"))
                        {
                            encodeline = i;
                            break;
                        }
                    }

                    // Filename Pattern
                    Match filenameMatch = Regex.Match(mime[fnameLine], ".*\\=(?<inner>.*)");
                    att.Name = filenameMatch.Groups["inner"].Value;

                    // encodeTyp Pattern
                    Match encodematch = Regex.Match(mime[encodeline], ".*\\:(?<inner>.*)");
                    att.Encodebase = encodematch.Groups["inner"].Value;

                    // Bytes Auslesen
                    String byteString = "";
                    for (int i = start; i < ende; i++)
                    {
                        byteString = byteString + mime[i];
                    }

                    Byte[] attachbytes = Convert.FromBase64String(byteString);
                    att.bytes = attachbytes;

                    // Att der Anhangsliste übergeben
                    attach.Add(att);
                }


                z++;

            }

            // nun den namen des Attachments
            



            return attach;
        }

        /// <summary>
        /// Gibt die Mail in dem Mime Format zurück, sollte ein Fehler auftretten bitte diese Option Ausführen."
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String getMimeMail(List<String> mime)
        {
            String mimestr="";

            foreach (String item in mime)
            {
                mimestr = mimestr + item;
            }

            return mimestr;
        }

        /// <summary>
        /// Gibt das Boundaray für die Anlagen zurück
        /// </summary>
        /// <param name="mime"></param>
        /// <returns></returns>
        public String getBoundarray(List<String> mime)
        {
            String boundarray = "";

            foreach (String item in mime)
            {
                if (item.Contains("boundary="))
                {
                    Match match = Regex.Match(item, ".*\\=(?<inner>.*)");
                    String boundarrayUnformattet = match.Groups["inner"].Value;
                    if (boundarrayUnformattet.Contains('"'))
                    {
                        Char[] chr = new Char[] {'\"'};
                        boundarray = boundarrayUnformattet.Replace(chr[0].ToString(),String.Empty);
                    }
                    else
                    {
                        boundarray = boundarrayUnformattet;
                    }
                }
            }

            return boundarray;

        }

        /// <summary>
        /// Gibt den Html Body aus.
        /// </summary>
        /// <param name="mime"></param>
        /// <param name="boundarray"></param>
        /// <returns></returns>
        public String HtmlBody(List<String> mime, String boundarray)
        {
            String body = "";
            int z = 0;
            int Start = 0;
            int ende = 0;
            

            foreach (String item in mime)
            {
                if (item.Contains("Content-Type: text/html;"))
                {
                    // Ab der gefunden zeile durchlaufen bis der Content Beginnt.
                    for (int i = z; i < mime.Count; i++)
                    {
                        // Wenn eine Leere Zeile Vorkommt, Start zeile festlegen
                        if (String.IsNullOrEmpty(mime[i]))
                        {
                            Start = i;
                            break;
                        }
                    }

                    // Ab der Startzeile durchlaufen bis das boundarray kommt
                    for (int i = Start; i < mime.Count; i++)
                    {
                        // wenn die Zeile das Boundarray Beiinhaltet End Zeile Deklarieren
                        if (mime[i].Contains(boundarray))
                        {
                            ende = i;
                            break;
                        }
                    }

                    // wenn fertig dann unterbrechen
                    break;
                    
                }

                // inkrement erhöhen
                z++;
                    
            }

            // body Ausgeben

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
        /// <param name="boundarray"></param>
        /// <returns></returns>
        public String PlainBody(List<String> mime, String boundarray)
        {
            String Plain="";
            int z = 0;
            int Start = 0;
            int ende = 0;

            foreach (String item in mime)
            {

                if(item.Contains("Content-Type: text/plain;"))
                {
                    // Start Zeile holen
                    for(int i = z; i < mime.Count ; i++)
                    {
                        if(String.IsNullOrEmpty(mime[i]))
                        {
                            Start = i;
                            break;
                        }
                    }

                    // End Zeile holen
                    for(int i = Start ; i < mime.Count ; i++)
                    {
                        if(mime[i].Contains(boundarray))
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
