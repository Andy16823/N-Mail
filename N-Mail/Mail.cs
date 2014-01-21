using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nMail
{
    public class Mail
    {
        public List<String> list;

        public Mail(UserInformation ui, int id)
        {
            list = new popClient().getMail(ui, id);
            getBoundarray();
            sethtmlBody();
            setPlainBody();
            setNoneMultipart();
            setSubject();
            setFrom();
            setReciver();
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
        public String Reciver { get; set; }
        public String boundarray { get; set; }


        public List<Attachment> Attachments { get; set; }
        public int AttachmentCount { get; set; }

        /// <summary>
        /// lädt das boundarray in die eigenschaft boundarray
        /// </summary>
        private void getBoundarray()
        {
            mime parser = new mime();
            this.boundarray = parser.getBoundarray(list);
            
        }

        /// <summary>
        /// lädt den html Content in die Eigenschaft BodyAsHtml
        /// </summary>
        private void sethtmlBody()
        {
            mime parser = new mime();
            if (parser.hasHtmlText(list) == true && this.boundarray != null)
            {
                this.hasHTMLBody = true;
                this.BodyAsHtml = parser.HtmlBody(list, parser.getBoundarray(list));
            }
        }

        /// <summary>
        /// lädt den plain Content in die Eigenschaft BodyAsHtml
        /// </summary>
        private void setPlainBody()
        {
            mime parser = new mime();
            if (parser.hasPlainText(list) == true && this.boundarray != null)
            {
                this.hasPlainTextBody = true;
                this.BodyAsPlainText = parser.PlainBody(list, this.boundarray);
            }
        }

        /// <summary>
        /// lädt den Body als none Multipart
        /// </summary>
        private void setNoneMultipart()
        {
            mime parser = new mime();

            if (parser.isMultipart(list) == false)
            {
                this.BodyAsNoneMultipart = parser.getNoneMultipart(list);
            }
        }

        /// <summary>
        /// lädt den Betreff der mail
        /// </summary>
        private void setSubject()
        {
            mime parser = new mime();
            this.Subject = parser.getSubject(list);
        }

        /// <summary>
        /// lädt den Absender in die From Eigenschaft
        /// </summary>
        private void setFrom()
        {
            mime parser = new mime();
            this.From = parser.getSender(list);
        }

        /// <summary>
        /// lädt den Absender in die Reciver Eigenschaft
        /// </summary>
        private void setReciver()
        {
            this.Reciver = new mime().getTo(list);
        }

        /// <summary>
        /// lädt den Boolenischen wert in die Eigenschaft hasMultipart
        /// </summary>
        private void setHasMultipart()
        {
            this.hasMultipart = new mime().isMultipart(list);
        }

        /// <summary>
        /// lädt den Boolenischen wert in die Eigenschaft hasMixedMultipart
        /// </summary>
        private void setHasMixedMultipart()
        {
            this.hasMixedMultipart = new mime().isMixedMultipart(list);
        }

        /// <summary>
        /// lädt den int Wert der Anzahl der Datei Anhänge
        /// </summary>
        private void getAttachmentCount()
        {
            mime parser = new mime();
            this.AttachmentCount = parser.getAttachmantCount(list);


        }

        /// <summary>
        /// lädt die liste der eigenschaften wenn der Count != 0 ist.
        /// </summary>
        private void getAttachments()
        {
            mime parser = new mime();

            if (parser.getAttachmantCount(list) != 0)
            {
                this.Attachments = parser.getAttachmants(list,boundarray);
            }
            
        }
    }
}
