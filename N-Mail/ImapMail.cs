using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nMail
{
    public class ImapMail
    {
        String imapMail;
        ImapParser parser;

        public ImapMail(String folder, UserInformation ui, int ID)
        {
            imapMail = new ImapClient(ui).GetMail(folder, ID);
            parser = new ImapParser(imapMail);

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

        public ImapMail(String mail)
        {
            parser = new ImapParser(mail);

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
            if(parser.HasBoundary())
            {
                this.Boundary = parser.GetBoundary();
            }
        }

        /// <summary>
        /// Lädt den HTML-Body in die Eigenschaft HtmlBody
        /// </summary>
        private void loadHtmlBody()
        {
            if (parser.HasHtmlBody() && !String.IsNullOrEmpty(Boundary))
            {
                this.HtmlBody = parser.GetBodyAsHtml(this.Boundary);
            }
        }

        /// <summary>
        /// Lädt den Plain-Body in die Eigenschaft PlainBody
        /// </summary>
        private void loadPlainBody()
        {
            if (parser.HasPlainBody() && !String.IsNullOrEmpty(Boundary))
            {
                this.PlainBody = parser.GetBodyAsPlain(this.Boundary);
            }
        }

        /// <summary>
        /// Lädt den Body der Pop3Mail in die Eigenschaft NoneMultipartBody
        /// </summary>
        private void loadNoneMultipartBody()
        {
            if (!parser.HasMultipart() && String.IsNullOrEmpty(Boundary))
            {
                this.NoneMultipartBody = parser.GetTextAsNoneMultiPart();
            }
        }

        /// <summary>
        /// Lädt den Absender der Pop3Mail in die Eigenschaft Sender
        /// </summary>
        private void loadSender()
        {
            this.Sender = parser.GetSender();
        }

        /// <summary>
        /// Lädt den Betreff der Pop3Mail in die Eigenschaft Subject
        /// </summary>
        private void loadSubject()
        {
            this.Subject = parser.GetSubject();
        }

        /// <summary>
        /// Lädt das Empfangsdatum in die Eigenschaft DeliveryDate
        /// </summary>
        private void loadDeliveryDate()
        {
            this.DeliveryDate = parser.GetDeliveryDate();
        }

        /// <summary>
        /// Lädt die Dateianhänge in die Eigenschaft Attachments
        /// </summary>
        private void loadAttachments()
        {
            if (parser.HasAttachments() && !String.IsNullOrEmpty(Boundary))
            {
                this.Attachments = parser.GetAttachments(this.Boundary);
            }
        }

        /// <summary>
        /// Lädt die Anzahl der enthaltenen Attachments in die Eigenschaft AttachmentCount
        /// </summary>
        private void loadAttachmentCount()
        {
            if (parser.HasAttachments())
            {
                this.AttachmentCount = parser.GetAttachmentCount();
            }
        }

        /// <summary>
        /// Lädt den Wert in die Eigenschaft HasMultipart, der angibt, ob die Pop3Mail vom Typ Multipart ist
        /// </summary>
        private void loadHasMultipart()
        {
            this.HasMultipart = parser.HasMultipart();
        }

        /// <summary>
        /// Lädt den Wert in die Eigenschaft HasMultipart, der angibt, ob die Pop3Mail vom Typ MixedMultipart ist
        /// </summary>
        private void loadHasMixedMultipart()
        {
            this.HasMixedMultipart = parser.HasMixedMultipart();
        }

        /// <summary>
        /// Lädt den Wert in die Eigenschaft HasHtmlBody, der angibt, ob die Pop3Mail einen HTML-Body besitzt
        /// </summary>
        private void loadHasHtmlBody()
        {
            this.HasHtmlBody = parser.HasHtmlBody();
        }

        /// <summary>
        /// Lädt den Wert in die Eigenschaft HasPlainBody, der angibt, ob die Pop3Mail einen Plain-Body besitzt
        /// </summary>
        private void loadHasPlainBody()
        {
            this.HasPlainBody = parser.HasPlainBody();
        }

    }
}
