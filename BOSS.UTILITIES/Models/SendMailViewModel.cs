using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOSS.UTILITIES.Models
{
    public class SendMailViewModel
    {
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Msg { get; set; }
        public string Username { get; set; }

        public ICollection<IFormFile> MailAttachment { get; set; }
    }
}
