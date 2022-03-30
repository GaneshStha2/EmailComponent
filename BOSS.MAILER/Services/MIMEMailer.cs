using BOSS.COMMON;
using BOSS.MAILER.Interface;
using BOSS.MAILER.Model;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using BOSS.COMMON.Logger;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Http;

namespace BOSS.MAILER.Services
{
    public class MIMEMailer : IMailer
    {
        private readonly MailSettings setting;
        public MIMEMailer(IOptionsMonitor<MailSettings> optionsMonitor)
        {
            setting = optionsMonitor.CurrentValue;
        }


        public ServiceResult<bool> SendMail(MailerRequest model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.ToAddress))
                {

                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse(setting.From));

                    foreach (string to in model.ToAddress.Split(','))
                        if (!string.IsNullOrWhiteSpace(to)) email.To.Add(MailboxAddress.Parse(to));
                    var builder = new BodyBuilder();

                    foreach (var a in model.Attachments)
                        builder.Attachments.Add(a.Filename, a.FileContent, new ContentType("application", a.FileType == MailAttachmentType.Excel ? "vnd.ms-excel" : "pdf"));
                    builder.TextBody = model.Msg;
                    email.Subject = model.Subject;

                    email.Body = builder.ToMessageBody();

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    // send email
                    using (var smtp = new SmtpClient())
                    {
                        smtp.CheckCertificateRevocation = false;
                        smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                        smtp.Connect(setting.HostName, setting.PortNo, SecureSocketOptions.StartTls);
                        smtp.Authenticate(setting.Username, setting.Password);
                        smtp.SendAsync(email);
                        smtp.Disconnect(true);

                    }

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

                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse(setting.From));

                    foreach (string to in model.ToAddress.Split(','))
                        if (!string.IsNullOrWhiteSpace(to)) email.To.Add(MailboxAddress.Parse(to));
                    var builder = new BodyBuilder();

                    foreach (var a in model.Attachments)
                        builder.Attachments.Add(a.Filename, a.FileContent, new ContentType("application", a.FileType == MailAttachmentType.Excel ? "vnd.ms-excel" : "pdf"));
                    builder.TextBody = model.Msg;
                    email.Subject = model.Subject;

                    email.Body = builder.ToMessageBody();

                    ServicePointManager.Expect100Continue = true;
                    // ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, ssl) => { return true; };
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };
                    //HttpClientHandler clientHandler = new HttpClientHandler();
                    //clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                    // send email
                    using (var smtp = new SmtpClient())
                    {
                        smtp.CheckCertificateRevocation = false;
                        //smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                        smtp.Connect(setting.HostName, setting.PortNo, false);
                        smtp.Authenticate(setting.Username, setting.Password);
                        await smtp.SendAsync(email);
                        smtp.Disconnect(true);

                    }

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
