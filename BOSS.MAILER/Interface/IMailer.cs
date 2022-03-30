using BOSS.COMMON;
using BOSS.MAILER.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOSS.MAILER.Interface
{
    public interface IMailer
    {
        public ServiceResult<bool> SendMail(MailerRequest model);
        public Task<ServiceResult<bool>> SendMailAsync(MailerRequest model);
    }
}
