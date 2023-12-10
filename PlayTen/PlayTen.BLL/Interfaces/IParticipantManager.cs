using PlayTen.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlayTen.BLL.Interfaces
{
    /// <summary>
    ///  Implements  operations for work with Training participants.
    /// </summary>
    public interface IParticipantManager
    {
        /// <summary>
        /// Create new Training participant.
        /// </summary>
        /// <returns>Status code of the subscribing on Training operation.</returns>
        /// <param name="targetTraining">The instance of Training</param>
        /// <param name="userId">The Id of logged in user</param>
        Task<int> SubscribeOnTrainingAsync(Training targetTraining, string userId);

        /// <summary>
        /// Create new Tournament participant.
        /// </summary>
        /// <returns>Status code of the subscribing on Tournament operation.</returns>
        /// <param name="targetTraining">The instance of Tournament</param>
        /// <param name="userId">The Id of logged in user</param>
        Task<int> SubscribeOnTournamentAsync(Tournament targetTournament, string userId);

        /// <summary>
        /// Delete Training participant.
        /// </summary>
        /// <returns>Status code of the unsubscribing on Training operation.</returns>
        /// <param name="targetTraining">The instance of Training</param>
        /// <param name="userId">The Id of logged in user</param>
        Task<int> UnSubscribeOnTrainingAsync(Training targetTraining, string userId);

        /// <summary>
        /// Delete Tournament participant.
        /// </summary>
        /// <returns>Status code of the unsubscribing on Tournament operation.</returns>
        /// <param name="targetTraining">The instance of Tournament</param>
        /// <param name="userId">The Id of logged in user</param>
        Task<int> UnSubscribeOnTournamentAsync(Tournament targetTournament, string userId);

        /// <summary>
        /// Change Training participant status to approved.
        /// </summary>
        /// <returns>Status code of the changing Training participant status operation.</returns>
        /// <param name="id">The Id of Training participant</param>
        Task<int> ChangeStatusToApprovedAsync(int id);

        /// <summary>
        /// Change Training participant status to under reviewed.
        /// </summary>
        /// <returns>Status code of the changing Training participant status operation.</returns>
        /// <param name="id">The Id of Training participant</param>
        Task<int> ChangeStatusToUnderReviewAsync(int id);

        /// <summary>
        /// Change Training participant status to rejected.
        /// </summary>
        /// <returns>Status code of the changing Training participant status operation.</returns>
        /// <param name="id">The Id of Training participant</param>
        Task<int> ChangeStatusToRejectedAsync(int id);

        /// <summary>
        /// Get list of Training participants by userId.
        /// </summary>
        /// <returns>List of Training participants.</returns>
        /// <param name="userId">The Id of logged in user</param>
        Task<IEnumerable<Participant>> GetParticipantsByUserIdAsync(string userId);
    }
}
