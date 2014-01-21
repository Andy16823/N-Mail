using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nMail
{
    public class imapMail
    {
        String imapmail;
        imapMime parser;

        public imapMail(String folder, UserInformation ui, int ID)
        {
            imapmail = new imapClient(ui).getmail(folder, ID);
            parser = new imapMime(imapmail);
            loadBoundarray();
            loadHtmlBody();
            loadPlainBody();
            loadNoneMultipartBody();
            loadSender();
            loadSubject();
            loadDeliveryDate();
            loadAttachments();
            loadAttachmantCount();
            loadHasMultipart();
            loadHasMixedMultipart();
            loadHasHtmlBody();
            loadHasPlainBody();
        }

        public String boundary { get; set; }
        public String HtmlBody { get; set; }
        public String PlainBody { get; set; }
        public String NoneMultipartBody { get; set; }
        public String Sender { get; set; }
        public String Subject { get; set; }
        public String DeliveryDate { get; set; }
        public List<Attachment> Attachmants { get; set; }
        public int AttachmantCount { get; set; }
        public Boolean HasMultipart { get; set; }
        public Boolean HasMixedMultipart { get; set; }
        public Boolean HasHtmlBody { get; set; }
        public Boolean HasPlainBody { get; set; }


        /// <summary>
        /// Ladet das boundary in die Eigenschaft boundary
        /// </summary>
        private void loadBoundarray()
        {
            if(parser.hasBoundaray()==true)
            {
                this.boundary = parser.boundarray();
            }
        }

        /// <summary>
        /// lädt den html body in die Eigenschaft HtmlBody
        /// </summary>
        private void loadHtmlBody()
        {
            if (parser.hasHtmlBody() == true && this.boundary != null)
            {
                this.HtmlBody = parser.bodyAsHtml(this.boundary);
            }
        }

        /// <summary>
        /// lädt den Plain Body in die Eigenschaft PlainBody
        /// </summary>
        private void loadPlainBody()
        {
            if (parser.hasPlainBody() == true && this.boundary != null)
            {
                this.PlainBody = parser.bodyAsPlain(this.boundary);
            }
        }

        /// <summary>
        /// lädt den Body der mail in die Eigenschaft NineMultipart
        /// </summary>
        private void loadNoneMultipartBody()
        {
            if (parser.hasMultipart() == false && this.boundary == null)
            {
                this.NoneMultipartBody = parser.noneMultiPart();
            }
        }

        /// <summary>
        /// lädt den Absender der Mail in die Eigenschaft sender
        /// </summary>
        private void loadSender()
        {
            this.Sender = parser.sender();
        }

        /// <summary>
        /// lädt den Betreff der Mail in die Eigenschaft Subject
        /// </summary>
        private void loadSubject()
        {
            this.Subject = parser.Subject();
        }

        /// <summary>
        /// lädt das Empfangsdatum in die Eigenschaft DeliveryDate
        /// </summary>
        private void loadDeliveryDate()
        {
            this.DeliveryDate = parser.DeliveryDate();
        }

        /// <summary>
        /// lädt die Datei Anhange in die EigenschaftAttachments
        /// </summary>
        private void loadAttachments()
        {
            if (parser.hasAttachmants() == true && this.boundary != null)
            {
                this.Attachmants = parser.Attachments(this.boundary);
            }
        }

        /// <summary>
        /// Lädt die Anzahl der Enthaltenen Attachmants in die Eigenschaft AttachmantCount
        /// </summary>
        private void loadAttachmantCount()
        {
            if (parser.hasAttachmants())
            {
                this.AttachmantCount = parser.AttachmentCount();
            }
        }

        /// <summary>
        /// lädt den wert in die Eigenschaft hasMultipart, der Angibt ob die Mail einen Multipart hat
        /// </summary>
        private void loadHasMultipart()
        {
            this.HasMultipart = parser.hasMultipart();
        }

        /// <summary>
        /// lädt den wert in die Eigenschaft HasMixedMultipart, der Angibt ob die Mail einen HasMixedMultipart hat
        /// </summary>
        private void loadHasMixedMultipart()
        {
            this.HasMixedMultipart = parser.hasMixedMultipart();
        }

        /// <summary>
        /// lädt den wert in die Eigenschaft HasHtmlBody, der Angibt ob die Mail einen HasHtmlBody besitzt
        /// </summary>
        private void loadHasHtmlBody()
        {
            this.HasHtmlBody = parser.hasHtmlBody();
        }

        /// <summary>
        /// lädt den wert in die Eigenschaft HasPlainBody, der Angibt ob die Mail einen HasPlainBody besitzt
        /// </summary>
        private void loadHasPlainBody()
        {
            this.HasPlainBody = parser.hasPlainBody();
        }


    }
}
