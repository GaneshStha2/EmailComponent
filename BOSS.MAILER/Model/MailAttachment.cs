using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOSS.MAILER.Model
{
    public class MailAttachment
    {
        public string Filename { get; set; }

        public byte[] FileContent { get; set; }

        public MailAttachmentType FileType { get; set; }
    }

    public enum MailAttachmentType
    {
        Pdf, Excel
    }
}
