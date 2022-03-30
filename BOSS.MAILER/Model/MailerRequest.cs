
using BOSS.MAILER.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOSS.MAILER.Model
{
    public class MailerRequest
    {
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Msg { get; set; }
        public string Username { get; set; }
        public MailAttachment[] Attachments { get; set; }
    }
}
