using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nMail
{
    public class Mail
    {
        public List<String> list;

        public Mail(UserInformation ui, int id)
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
            Mime parser = new Mime();
            this.Boundary = parser.getBoundary(list);
            
        }

        /// <summary>
        /// Setzt den HTML-Content
        /// </summary>
        private void setHtmlBody()
        {
            Mime parser = new Mime();
            if (parser.hasHtmlText(list) == true && this.Boundary != null)
            {
                this.hasHTMLBody = true;
                this.BodyAsHtml = parser.HtmlBody(list, parser.getBoundary(list));
            }
        }

        /// <summary>
        /// Setzt den Plain-Content
        /// </summary>
        private void setPlainBody()
        {
            Mime parser = new Mime();
            if (parser.hasPlainText(list) == true && this.Boundary != null)
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
            Mime parser = new Mime();

            if (parser.isMultipart(list) == false)
            {
                this.BodyAsNoneMultipart = parser.getNoneMultipart(list);
            }
        }

        /// <summary>
        /// Setzt den Betreff der Mail
        /// </summary>
        private void setSubject()
        {
            Mime parser = new Mime();
            this.Subject = parser.getSubject(list);
        }

        /// <summary>
        /// Setzt den Absender
        /// </summary>
        private void setFrom()
        {
            Mime parser = new Mime();
            this.From = parser.getSender(list);
        }

        /// <summary>
        /// Setzt den Empfänger
        /// </summary>
        private void setReceiver()
        {
            this.Receiver = new Mime().getTo(list);
        }

        /// <summary>
        /// Setzt die Eigenschaft hasMultipart
        /// </summary>
        private void setHasMultipart()
        {
            this.hasMultipart = new Mime().isMultipart(list);
        }

        /// <summary>
        /// Setzt die Eigenschaft hasMixedMultipart
        /// </summary>
        private void setHasMixedMultipart()
        {
            this.hasMixedMultipart = new Mime().isMixedMultipart(list);
        }

        /// <summary>
        /// Lädt die Anzahl der Anhänge
        /// </summary>
        private void getAttachmentCount()
        {
            Mime parser = new Mime();
            this.AttachmentCount = parser.getAttachmentCount(list);
        }

        /// <summary>
        /// Lädt die Liste der Anhänge
        /// </summary>
        private void getAttachments()
        {
            Mime parser = new Mime();

            if (parser.getAttachmentCount(list) != 0)
            {
                this.Attachments = parser.getAttachments(list,this.Boundary);
            }
            
        }
    }
}
