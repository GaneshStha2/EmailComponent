using BOSS.COMMON;
using BOSS.MAILER.Interface;
using BOSS.MAILER.Model;
using BOSS.UTILITIES.Filters;
using BOSS.UTILITIES.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BOSS.UTILITIES.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]

    //[BossApiAccessFilter]
    public class MailerApiController : ControllerBase
    {

        IMailer _mailer;
        public MailerApiController(IMailer mailer)
        {
            _mailer = mailer;
        }

        [HttpPost]
        [Route("SendMail")]
        public async Task<ServiceResult<bool>> SendMail([FromForm] SendMailViewModel model)
        {
            var mailObject = new MailerRequest()
            {
                ToAddress = model.ToAddress,
                Msg = model.Msg,
                Subject = model.Subject,
                Username = model.Username,
            };
            List<MailAttachment> mailAttachment = new List<MailAttachment>();
            if (model.MailAttachment != null)
            {
                foreach (var file in model.MailAttachment)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms)
;
                            var fileBytes = ms.ToArray();
                            mailAttachment.Add(new MailAttachment()
                            {
                                FileContent = fileBytes,
                                Filename = file.FileName,
                                FileType = file.ContentType == "application/vnd.ms-excel" ? MailAttachmentType.Excel : MailAttachmentType.Pdf
                            });
                        }

                    }
                }
            }

            mailObject.Attachments = mailAttachment.ToArray();
            var result = await _mailer.SendMailAsync(mailObject);
            return result;
        }
    }
}