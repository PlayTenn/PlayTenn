using PlayTen.BLL.Models;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
//using MimeKit;

namespace PlayTen.BLL.Interfaces.EmailSending
{
    public interface IEmailSendingService
    {
        /// <summary>
        /// Composes and sends email 
        /// </summary>
        /// <param name="reciever">Reciever of the message</param>
        /// <param name="subject">Subject of the message</param>
        /// <param name="body">Text of the message</param>
        /// <param name="senderName">Our organisation name to be shown in letter</param>
        /// <returns>Result of sending email</returns>
        Task<bool> SendEmailAsync(string reciever, string subject, string body);

        Task SendEmailAsync(EmailModel message, MailAddress reciever);
    }
}
