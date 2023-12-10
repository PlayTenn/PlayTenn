using Microsoft.Extensions.Options;
using PlayTen.BLL.Interfaces;
using PlayTen.BLL.Interfaces.EmailSending;
using PlayTen.BLL.Models;
using PlayTen.BLL.Services.EmailSending;
using PlayTen.BLL.Services.Jwt;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;

namespace PlayTen.BLL.Services
{
    public class EmailSendingService : IEmailSendingService
    {
        private readonly ILoggerService<EmailSendingService> loggerService;
        private readonly EmailServiceSettings Settings;

        public EmailSendingService(
            IOptions<EmailServiceSettings> settings, ILoggerService<EmailSendingService> loggerService
        )
        {
            Settings = settings.Value;
            this.loggerService = loggerService;
        }

        public async Task<bool> SendEmailAsync(string reciever, string subject, string body)
        {
            var SMTPServer = Settings.SMTPServer;
            var Port = Settings.Port;
            var SenderName = Settings.SenderName;
            var Sender = Settings.Sender;

            
            MailAddress from = new MailAddress(SenderName, Sender);
            MailAddress to = new MailAddress(reciever);
            var emailMessage = new MailMessage(from, to);
            emailMessage.Subject = subject;
            emailMessage.Body = body;
            try
            {
                var client = new SmtpClient(SMTPServer, Port);
                await client.SendMailAsync(emailMessage);
                emailMessage.Dispose();
            }
            catch (Exception exс)
            {
                loggerService.LogError(exс.Message);
                return false;
            }
            return true;
        }

        public async Task SendEmailAsync(EmailModel message, MailAddress reciever)
        {
            var SMTPServer = Settings.SMTPServer;
            var Port = Settings.Port;
            var SenderName = Settings.SenderName;
            var Sender = Settings.Sender;
            var Password = Settings.Password;

            
            MailAddress from = new MailAddress(Sender, SenderName);
            var emailMessage = new MailMessage(from, reciever);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = message.Message;
            emailMessage.IsBodyHtml = true;

            try
            {
                var client = new SmtpClient {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Host = SMTPServer,
                    Port = Port,
                    Credentials = new NetworkCredential(Sender, Password)
                };
                await client.SendMailAsync(emailMessage);
                emailMessage.Dispose();
            }
            catch (Exception exс)
            {
                loggerService.LogError(exс.Message);
            }
        }
    }
}
