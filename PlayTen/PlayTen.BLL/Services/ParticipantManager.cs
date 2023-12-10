using PlayTen.BLL.Interfaces;
using PlayTen.DAL.Entities;
using PlayTen.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PlayTen.BLL.Interfaces.EmailSending;
using System.Net.Mail;

namespace PlayTen.BLL.Services
{
    public class ParticipantManager : IParticipantManager
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IParticipantStatusManager _participantStatusManager;
        private readonly IEmailContentService _emailContentService;
        private readonly IEmailSendingService _emailSendingService;
        private readonly UserManager<User> _userManager;


        public ParticipantManager(IRepositoryWrapper repoWrapper, IParticipantStatusManager participantStatusManager, IEmailSendingService emailSendingService, IEmailContentService emailContentService, UserManager<User> userManager)
        {
            _repoWrapper = repoWrapper;
            _participantStatusManager = participantStatusManager;
            _emailSendingService = emailSendingService;
            _emailContentService = emailContentService;
            _userManager = userManager;
        }

        /// <inheritdoc />
        public async Task<int> SubscribeOnTrainingAsync(Training targetTraining, string userId)
        {
            try
            {
                int undeterminedStatus = await _participantStatusManager.GetStatusIdAsync("Розглядається");
                await _repoWrapper.Participant.CreateAsync(new Participant() { ParticipantStatusId = undeterminedStatus, TrainingId = targetTraining.Id, UserId = userId });
                await _repoWrapper.SaveAsync();
                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<int> SubscribeOnTournamentAsync(Tournament targetTournament, string userId)
        {
            try
            {
                int participantStatus = await _participantStatusManager.GetStatusIdAsync("Учасник");
                await _repoWrapper.Participant.CreateAsync(new Participant() { ParticipantStatusId = participantStatus, TournamentId = targetTournament.Id, UserId = userId, TrainingId = null });
                await _repoWrapper.SaveAsync();
                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<int> UnSubscribeOnTrainingAsync(Training targetTraining, string userId)
        {
            try
            {
                int rejectedStatus = await _participantStatusManager.GetStatusIdAsync("Відмовлено");
                Participant participantToDelete = await _repoWrapper.Participant
                    .GetFirstAsync(predicate: p => p.TrainingId == targetTraining.Id && p.UserId == userId);
                if (participantToDelete.ParticipantStatusId == rejectedStatus)
                {
                    return StatusCodes.Status409Conflict;
                }
                _repoWrapper.Participant.Delete(participantToDelete);
                await _repoWrapper.SaveAsync();

                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<int> UnSubscribeOnTournamentAsync(Tournament targetTournament, string userId)
        {
            try
            {
                int rejectedStatus = await _participantStatusManager.GetStatusIdAsync("Відмовлено");
                Participant participantToDelete = await _repoWrapper.Participant
                    .GetFirstAsync(predicate: p => p.TournamentId == targetTournament.Id && p.UserId == userId);
                if (participantToDelete.ParticipantStatusId == rejectedStatus)
                {
                    return StatusCodes.Status409Conflict;
                }
                _repoWrapper.Participant.Delete(participantToDelete);
                await _repoWrapper.SaveAsync();

                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<int> ChangeStatusToApprovedAsync(int id)
        {
            try
            {
                var participant = await _repoWrapper.Participant
                    .GetFirstAsync(predicate: p => p.ID == id);
                int approvedStatus = await _participantStatusManager.GetStatusIdAsync("Прийнято");
                participant.ParticipantStatusId = approvedStatus;
                _repoWrapper.Participant.Update(participant);
                await _repoWrapper.SaveAsync();

                var training = await _repoWrapper.Training.GetFirstAsync(predicate: p => p.Id == participant.TrainingId);
                var participantUser = await _userManager.FindByIdAsync(participant.UserId);
                var trainingOwner = await _userManager.FindByIdAsync(training.OwnerId);
                var participantUserName = $"{participantUser.Name} {participantUser.Surname}";

                var recieverParticipant = new MailAddress(participantUser.Email, participantUserName);
                var messageToParticipant = _emailContentService.GetTrainingParticipantStatusChangedEmail(training.Name, "Прийнято", trainingOwner.HowToConnect);
                await _emailSendingService.SendEmailAsync(messageToParticipant, recieverParticipant);

                var recieverOwner = new MailAddress(trainingOwner.Email, $"{trainingOwner.Name} {trainingOwner.Surname}");
                var messageToOwner = _emailContentService.GetApprovedParticipantContactsEmail(training.Name, participantUserName, participantUser.HowToConnect);
                await _emailSendingService.SendEmailAsync(messageToOwner, recieverOwner);


                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<int> ChangeStatusToUnderReviewAsync(int id)
        {
            try
            {
                var participant = await _repoWrapper.Participant
                    .GetFirstAsync(predicate: p => p.ID == id);
                int undeterminedStatus = await _participantStatusManager.GetStatusIdAsync("Розглядається");
                participant.ParticipantStatusId = undeterminedStatus;
                _repoWrapper.Participant.Update(participant);
                await _repoWrapper.SaveAsync();

                var training = await _repoWrapper.Training.GetFirstAsync(predicate: p => p.Id == participant.TrainingId);
                var participantUser = await _userManager.FindByIdAsync(participant.UserId);
                var trainingOwner = await _userManager.FindByIdAsync(training.OwnerId);
                var participantUserName = $"{participantUser.Name} {participantUser.Surname}";

                var recieverParticipant = new MailAddress(participantUser.Email, participantUserName);
                var messageToParticipant = _emailContentService.GetTrainingParticipantStatusChangedEmail(training.Name, "Розглядається", trainingOwner.HowToConnect);
                await _emailSendingService.SendEmailAsync(messageToParticipant, recieverParticipant);

                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<int> ChangeStatusToRejectedAsync(int id)
        {
            try
            {
                var participant = await _repoWrapper.Participant
                    .GetFirstAsync(predicate: p => p.ID == id);
                int rejectedStatus = await _participantStatusManager.GetStatusIdAsync("Відмовлено");
                participant.ParticipantStatusId = rejectedStatus;
                _repoWrapper.Participant.Update(participant);
                await _repoWrapper.SaveAsync();

                var training = await _repoWrapper.Training.GetFirstAsync(predicate: p => p.Id == participant.TrainingId);
                var participantUser = await _userManager.FindByIdAsync(participant.UserId);
                var trainingOwner = await _userManager.FindByIdAsync(training.OwnerId);
                var participantUserName = $"{participantUser.Name} {participantUser.Surname}";

                var recieverParticipant = new MailAddress(participantUser.Email, participantUserName);
                var messageToParticipant = _emailContentService.GetTrainingParticipantStatusChangedEmail(training.Name, "Відмовлено", trainingOwner.HowToConnect);
                await _emailSendingService.SendEmailAsync(messageToParticipant, recieverParticipant);

                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Participant>> GetParticipantsByUserIdAsync(string userId)
        {

            var participants = await _repoWrapper.Participant
                .GetAllAsync(
                    predicate: p => p.UserId == userId,
                    include: source => source.Include(i => i.Training)
                );

            return participants;
        }
    }
}
