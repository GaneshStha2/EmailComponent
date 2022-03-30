using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOSS.COMMON.Core
{
    public class ApiSettings
    {
        public string AccessID { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string AllowedIPs { get; set; }
    }
}
