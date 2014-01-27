using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nMail
{
    public class ImapMail
    {
        String imapMail;
        ImapMime parser;

        public ImapMail(String folder, UserInformation ui, int ID)
        {
            imapMail = new ImapClient(ui).getMail(folder, ID);
            parser = new ImapMime(imapMail);

            loadBoundary();
            loadHtmlBody();
            loadPlainBody();
            loadNoneMultipartBody();
            loadSender();
            loadSubject();
            loadDeliveryDate();
            loadAttachments();
            loadAttachmentCount();
            loadHasMultipart();
            loadHasMixedMultipart();
            loadHasHtmlBody();
            loadHasPlainBody();
        }

        public String Boundary { get; set; }
        public String HtmlBody { get; set; }
        public String PlainBody { get; set; }
        public String NoneMultipartBody { get; set; }
        public String Sender { get; set; }
        public String Subject { get; set; }
        public String DeliveryDate { get; set; }
        public List<Attachment> Attachments { get; set; }
        public int AttachmentCount { get; set; }
        public Boolean HasMultipart { get; set; }
        public Boolean HasMixedMultipart { get; set; }
        public Boolean HasHtmlBody { get; set; }
        public Boolean HasPlainBody { get; set; }


        /// <summary>
        /// Ladet das Boundary in die Eigenschaft boundary
        /// </summary>
        private void loadBoundary()
        {
            if(parser.hasBoundary())
            {
                this.Boundary = parser.getBoundary();
            }
        }

        /// <summary>
        /// Lädt den HTML-Body in die Eigenschaft HtmlBody
        /// </summary>
        private void loadHtmlBody()
        {
            if (parser.hasHtmlBody() && !String.IsNullOrEmpty(Boundary))
            {
                this.HtmlBody = parser.getBodyAsHtml(this.Boundary);
            }
        }

        /// <summary>
        /// Lädt den Plain-Body in die Eigenschaft PlainBody
        /// </summary>
        private void loadPlainBody()
        {
            if (parser.hasPlainBody() && !String.IsNullOrEmpty(Boundary))
            {
                this.PlainBody = parser.getBodyAsPlain(this.Boundary);
            }
        }

        /// <summary>
        /// Lädt den Body der Mail in die Eigenschaft NoneMultipartBody
        /// </summary>
        private void loadNoneMultipartBody()
        {
            if (!parser.hasMultipart() && String.IsNullOrEmpty(Boundary))
            {
                this.NoneMultipartBody = parser.getTextAsNoneMultiPart();
            }
        }

        /// <summary>
        /// Lädt den Absender der Mail in die Eigenschaft Sender
        /// </summary>
        private void loadSender()
        {
            this.Sender = parser.getSender();
        }

        /// <summary>
        /// Lädt den Betreff der Mail in die Eigenschaft Subject
        /// </summary>
        private void loadSubject()
        {
            this.Subject = parser.getSubject();
        }

        /// <summary>
        /// Lädt das Empfangsdatum in die Eigenschaft DeliveryDate
        /// </summary>
        private void loadDeliveryDate()
        {
            this.DeliveryDate = parser.getDeliveryDate();
        }

        /// <summary>
        /// Lädt die Dateianhänge in die Eigenschaft Attachments
        /// </summary>
        private void loadAttachments()
        {
            if (parser.hasAttachments() && !String.IsNullOrEmpty(Boundary))
            {
                this.Attachments = parser.getAttachments(this.Boundary);
            }
        }

        /// <summary>
        /// Lädt die Anzahl der enthaltenen Attachments in die Eigenschaft AttachmentCount
        /// </summary>
        private void loadAttachmentCount()
        {
            if (parser.hasAttachments())
            {
                this.AttachmentCount = parser.getAttachmentCount();
            }
        }

        /// <summary>
        /// Lädt den Wert in die Eigenschaft HasMultipart, der angibt, ob die Mail vom Typ Multipart ist
        /// </summary>
        private void loadHasMultipart()
        {
            this.HasMultipart = parser.hasMultipart();
        }

        /// <summary>
        /// Lädt den Wert in die Eigenschaft HasMultipart, der angibt, ob die Mail vom Typ MixedMultipart ist
        /// </summary>
        private void loadHasMixedMultipart()
        {
            this.HasMixedMultipart = parser.hasMixedMultipart();
        }

        /// <summary>
        /// Lädt den Wert in die Eigenschaft HasHtmlBody, der angibt, ob die Mail einen HTML-Body besitzt
        /// </summary>
        private void loadHasHtmlBody()
        {
            this.HasHtmlBody = parser.hasHtmlBody();
        }

        /// <summary>
        /// Lädt den Wert in die Eigenschaft HasPlainBody, der angibt, ob die Mail einen Plain-Body besitzt
        /// </summary>
        private void loadHasPlainBody()
        {
            this.HasPlainBody = parser.hasPlainBody();
        }

    }
}
