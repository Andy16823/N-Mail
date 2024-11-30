using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nMail
{
    public class Pop3Mail
    {
        public List<String> list;

        public Pop3Mail(UserInformation ui, int id)
        {
            list = new PopClient().getMail(ui, id);
            getBoundary();
            setHtmlBody();
            setPlainBody();
            setNoneMultipart();
            setSubject();
            setFrom();
            setReceiver();
            setHasMultipart();
            setHasMixedMultipart();
            getAttachmentCount();
            getAttachments();
        }


        public Boolean hasHTMLBody { get; set; }
        public Boolean hasPlainTextBody { get; set; }
        public Boolean hasAttachments { get; set; }
        public Boolean hasMultipart { get; set; }
        public Boolean hasMixedMultipart { get; set; }

        public String BodyAsHtml { get; set; }
        public String BodyAsPlainText { get; set; }
        public String BodyAsNoneMultipart { get; set; }
        public String Subject { get; set; }
        public String From { get; set; }
        public String Receiver { get; set; }
        public String Boundary { get; set; }


        public List<Attachment> Attachments { get; set; }
        public int AttachmentCount { get; set; }

        /// <summary>
        /// Lädt das Boundary in die Eigenschaft Boundary
        /// </summary>
        private void getBoundary()
        {
            POP3Parser parser = new POP3Parser();
            this.Boundary = parser.GetBoundary(list);
            
        }

        /// <summary>
        /// Setzt den HTML-Content
        /// </summary>
        private void setHtmlBody()
        {
            POP3Parser parser = new POP3Parser();
            if (parser.HasHtmlText(list) == true && this.Boundary != null)
            {
                this.hasHTMLBody = true;
                this.BodyAsHtml = parser.HtmlBody(list, parser.GetBoundary(list));
            }
        }

        /// <summary>
        /// Setzt den Plain-Content
        /// </summary>
        private void setPlainBody()
        {
            POP3Parser parser = new POP3Parser();
            if (parser.HasPlainText(list) == true && this.Boundary != null)
            {
                this.hasPlainTextBody = true;
                this.BodyAsPlainText = parser.PlainBody(list, this.Boundary);
            }
        }

        /// <summary>
        /// Setzt den Body als NoneMultipart
        /// </summary>
        private void setNoneMultipart()
        {
            POP3Parser parser = new POP3Parser();

            if (parser.IsMultipart(list) == false)
            {
                this.BodyAsNoneMultipart = parser.GetNoneMultipart(list);
            }
        }

        /// <summary>
        /// Setzt den Betreff der Pop3Mail
        /// </summary>
        private void setSubject()
        {
            POP3Parser parser = new POP3Parser();
            this.Subject = parser.GetSubject(list);
        }

        /// <summary>
        /// Setzt den Absender
        /// </summary>
        private void setFrom()
        {
            POP3Parser parser = new POP3Parser();
            this.From = parser.GetSender(list);
        }

        /// <summary>
        /// Setzt den Empfänger
        /// </summary>
        private void setReceiver()
        {
            this.Receiver = new POP3Parser().GetTo(list);
        }

        /// <summary>
        /// Setzt die Eigenschaft HasMultipart
        /// </summary>
        private void setHasMultipart()
        {
            this.hasMultipart = new POP3Parser().IsMultipart(list);
        }

        /// <summary>
        /// Setzt die Eigenschaft HasMixedMultipart
        /// </summary>
        private void setHasMixedMultipart()
        {
            this.hasMixedMultipart = new POP3Parser().IsMixedMultipart(list);
        }

        /// <summary>
        /// Lädt die Anzahl der Anhänge
        /// </summary>
        private void getAttachmentCount()
        {
            POP3Parser parser = new POP3Parser();
            this.AttachmentCount = parser.GetAttachmentCount(list);
        }

        /// <summary>
        /// Lädt die Liste der Anhänge
        /// </summary>
        private void getAttachments()
        {
            POP3Parser parser = new POP3Parser();

            if (parser.GetAttachmentCount(list) != 0)
            {
                this.Attachments = parser.GetAttachments(list,this.Boundary);
            }
            
        }
    }
}
