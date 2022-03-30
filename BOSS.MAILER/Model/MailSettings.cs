using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOSS.MAILER.Model
{
    public class MailSettings
    {
        public string HostName { get; set; }

        public int PortNo { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool SSL { get; set; }

        public string From { get; set; }

        public string DisplayName { get; set; }


    }
}
