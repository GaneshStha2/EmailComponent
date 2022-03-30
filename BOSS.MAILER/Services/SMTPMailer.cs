using BOSS.COMMON;
using BOSS.COMMON.Logger;
using BOSS.MAILER.Interface;
using BOSS.MAILER.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BOSS.MAILER.Services
{
    public class SMTPMailer : IMailer
    {
        private readonly MailSettings setting;
        public SMTPMailer(IOptionsMonitor<MailSettings> optionsMonitor)
        {
            setting = optionsMonitor.CurrentValue;
        }

        public ServiceResult<bool> SendMail(MailerRequest model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.ToAddress))
                {

                    MailMessage mail = new MailMessage
                    {
                        From = new MailAddress(setting.From, setting.DisplayName),
                        Subject = model.Subject,
                        Body = model.Msg,
                        IsBodyHtml = true
                    };

                    foreach (string to in model.ToAddress.Split(','))
                        if (!string.IsNullOrWhiteSpace(to)) mail.To.Add(new MailAddress(to));

                    foreach (var a in model.Attachments)
                        mail.Attachments.Add(new Attachment(new MemoryStream(a.FileContent), a.Filename, a.FileType == MailAttachmentType.Excel ? "application/vnd.ms-excel" : "application/pdf"));
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    SmtpClient smtp = new SmtpClient
                    {
                        Host = setting.HostName,
                        Port = setting.PortNo,
                        EnableSsl = setting.SSL,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(setting.Username, setting.Password)
                    };
                    smtp.Send(mail);

                    return new ServiceResult<bool>()
                    {
                        Data = true,
                        Message = "Mail Send Successfully",
                        Status = ResultStatus.Ok
                    };

                }
                else
                {
                    return new ServiceResult<bool>()
                    {
                        Data = false,
                        Message = "something went wrong",
                        Status = ResultStatus.processError
                    };
                }
            }
            catch (Exception ex)
            {
                var text = JsonConvert.SerializeObject(ex);
                ExceptionLogger.Write(text);
                return new ServiceResult<bool>()
                {
                    Data = false,
                    Message = "Send Mail Error",
                    Status = ResultStatus.unHandeledError
                };
            }


        }


        public async Task<ServiceResult<bool>> SendMailAsync(MailerRequest model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.ToAddress))
                {

                    MailMessage mail = new MailMessage
                    {
                        From = new MailAddress(setting.From, setting.DisplayName),
                        Subject = model.Subject,
                        Body = model.Msg,
                        IsBodyHtml = true
                    };

                    foreach (string to in model.ToAddress.Split(','))
                        if (!string.IsNullOrWhiteSpace(to)) mail.To.Add(new MailAddress(to));

                    foreach (var a in model.Attachments)
                        mail.Attachments.Add(new Attachment(new MemoryStream(a.FileContent), a.Filename, a.FileType == MailAttachmentType.Excel ? "application/vnd.ms-excel" : "application/pdf"));
                    ServicePointManager.Expect100Continue = true;
                    //  ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, ssl) => { return true; };
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };
                    //HttpClientHandler clientHandler = new HttpClientHandler();
                    //clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                    SmtpClient smtp = new SmtpClient
                    {
                        Host = setting.HostName,
                        Port = setting.PortNo,
                        EnableSsl = setting.SSL,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(setting.Username, setting.Password)
                    };
                    await smtp.SendMailAsync(mail);

                    return new ServiceResult<bool>()
                    {
                        Data = true,
                        Message = "Mail Send Successfully",
                        Status = ResultStatus.Ok
                    };

                }
                else
                {
                    return new ServiceResult<bool>()
                    {
                        Data = false,
                        Message = "something went wrong",
                        Status = ResultStatus.processError
                    };
                }
            }
            catch (Exception ex)
            {
                var text = JsonConvert.SerializeObject(ex);
                ExceptionLogger.Write(text);

                return new ServiceResult<bool>()
                {
                    Data = false,
                    Message = "Send Mail Error",
                    Status = ResultStatus.unHandeledError
                };
            }
        }
    }
}
