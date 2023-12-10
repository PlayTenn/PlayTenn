using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PlayTen.BLL.DTO;
using PlayTen.BLL.Models;

namespace PlayTen.BLL.Interfaces.EmailSending
{
    public interface IEmailContentService
    {
        /// <summary>
        /// Get email for registration confirmation
        /// </summary>
        /// <param name="confirmationLink">Registration confirmation link</param>
        /// <returns>Email content</returns>
        EmailModel GetAuthRegisterEmail(string confirmationLink);
        EmailModel GetNewTrainingParticipantEmail(string username);
        EmailModel GetTrainingParticipantStatusChangedEmail(string trainingName, string status, string howToConnect = null);
        EmailModel GetApprovedParticipantContactsEmail(string trainingName, string participantUserName, string howToConnect = null);

    }
}
