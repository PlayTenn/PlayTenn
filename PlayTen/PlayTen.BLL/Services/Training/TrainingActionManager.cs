using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;
using PlayTen.BLL.Interfaces.EmailSending;
using PlayTen.DAL.Entities;
using PlayTen.DAL.Repositories;

using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Policy;
using System.Threading.Tasks;

namespace PlayTen.BLL.Services
{
    public class TrainingActionManager : ITrainingActionManager
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IParticipantStatusManager _participantStatusManager;
        private readonly IParticipantManager _participantManager;
        private readonly IEmailContentService _emailContentService;
        private readonly IEmailSendingService _emailSendingService;

        public bool isUserTrainingOwner;
        public bool isUserParticipant;
        public bool isUserApprovedParticipant;
        public bool isUserUndeterminedParticipant;
        public bool isUserRejectedParticipant;

        public TrainingActionManager(UserManager<User> userManager, IRepositoryWrapper repoWrapper, IMapper mapper,
            IParticipantStatusManager participantStatusManager, IParticipantManager participantManager, IEmailSendingService emailSendingService, IEmailContentService emailContentService)
        {
            _userManager = userManager;
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _participantStatusManager = participantStatusManager;
            _participantManager = participantManager;
            _emailSendingService = emailSendingService;
            _emailContentService = emailContentService;
        }

        ///// <inheritdoc />
        public async Task<IEnumerable<TrainingDTO>> GetTrainingsAsync(User user)
        {
            var trainings = await _repoWrapper.Training.GetAllAsync(include:
                    source => source
                        .Include(e => e.Participants)
                        .ThenInclude(p => p.User)
                        .Include(e => e.Participants)
                        .ThenInclude(p => p.ParticipantStatus)
                        .Include(a => a.Place));

            return await GetTrainingDtosAsync(trainings, user);
        }

        /// <inheritdoc />
        public async Task<TrainingDTO> GetTrainingInfoAsync(int id, User user)
        {
            int approvedStatus = await _participantStatusManager.GetStatusIdAsync("Прийнято");
            int undeterminedStatus = await _participantStatusManager.GetStatusIdAsync("Розглядається");
            int rejectedStatus = await _participantStatusManager.GetStatusIdAsync("Відмовлено");
            var userRoles = await _userManager.GetRolesAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);


            var targetTraining = await _repoWrapper.Training
                .GetFirstAsync(
                    e => e.Id == id,
                    source => source
                        .Include(e => e.Participants)
                        .ThenInclude(p => p.User)
                        .ThenInclude(u => u.TennisLevel)
                        .Include(e => e.Participants)
                        .ThenInclude(p => p.ParticipantStatus)
                        .Include(a => a.Place)
                );

            if (targetTraining.Participants != null)
            {
                isUserTrainingOwner = targetTraining.OwnerId.Equals(userId);
                isUserParticipant = targetTraining.Participants.Any(p => p.UserId == userId);
                isUserApprovedParticipant = targetTraining.Participants.Any(p => p.UserId == userId && p.ParticipantStatusId == approvedStatus);
                isUserUndeterminedParticipant = targetTraining.Participants.Any(p => p.UserId == userId && p.ParticipantStatusId == undeterminedStatus);
                isUserRejectedParticipant = targetTraining.Participants.Any(p => p.UserId == userId && p.ParticipantStatusId == rejectedStatus);
            }

            var dto = new TrainingDTO()
            {
                Training = _mapper.Map<Training, TrainingInfoDTO>(targetTraining),
                IsUserTrainingOwner = isUserTrainingOwner,
                IsUserParticipant = isUserParticipant,
                IsUserApprovedParticipant = isUserApprovedParticipant,
                IsUserUndeterminedParticipant = isUserUndeterminedParticipant,
                IsUserRejectedParticipant = isUserRejectedParticipant
            };

            if (!dto.IsUserTrainingOwner)
            {
                dto.Training.Participants = dto.Training.Participants.Where(p => p.StatusId == approvedStatus);
            }

            return dto;
        }

        /// <inheritdoc />
        public async Task<int> DeleteTrainingAsync(int id)
        {
            try
            {
                Training objectToDelete = await _repoWrapper.Training
                    .GetFirstAsync(e => e.Id == id);
                _repoWrapper.Training.Delete(objectToDelete);
                await _repoWrapper.SaveAsync();
                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<int> SubscribeOnTrainingAsync(int id, User user)
        {
            try
                {
                Training targetTraining = await _repoWrapper.Training
                    .GetFirstAsync(e => e.Id == id);
                var userId = await _userManager.GetUserIdAsync(user);
                int result = await _participantManager.SubscribeOnTrainingAsync(targetTraining, userId);

                if (result == StatusCodes.Status200OK)
                {
                    var owner = await _userManager.FindByIdAsync(targetTraining.OwnerId);
                    var reciever = new MailAddress(owner.Email, $"{owner.Name} {owner.Surname}");
                    var message = _emailContentService.GetNewTrainingParticipantEmail($"{user.Name} {user.Surname}");
                    await _emailSendingService.SendEmailAsync(message, reciever);
                }

                return result;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<int> UnSubscribeOnTrainingAsync(int id, User user)
        {
            try
            {
                Training targetTraining = await _repoWrapper.Training
                    .GetFirstAsync(e => e.Id == id);
                var userId = await _userManager.GetUserIdAsync(user);
                int result = await _participantManager.UnSubscribeOnTrainingAsync(targetTraining, userId);
                return result;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<int> ApproveParticipantAsync(int id)
        {
            var result = await _participantManager.ChangeStatusToApprovedAsync(id);
            return result;
        }

        /// <inheritdoc />
        public async Task<int> UnderReviewParticipantAsync(int id)
        {
            return await _participantManager.ChangeStatusToUnderReviewAsync(id);
        }

        /// <inheritdoc />
        public async Task<int> RejectParticipantAsync(int id)
        {
            return await _participantManager.ChangeStatusToRejectedAsync(id);
        }


        private async Task<List<TrainingDTO>> GetTrainingDtosAsync(IEnumerable<Training> Trainings, User user)
        {
            int approvedStatus = await _participantStatusManager.GetStatusIdAsync("Прийнято");
            int undeterminedStatus = await _participantStatusManager.GetStatusIdAsync("Розглядається");
            int rejectedStatus = await _participantStatusManager.GetStatusIdAsync("Відмовлено");
            var userRoles = await _userManager.GetRolesAsync(user);

            var userId = _userManager.GetUserIdAsync(user).Result;

            return Trainings
                .Select(tr => {
                    tr.User = _userManager.FindByIdAsync(tr.OwnerId).Result;
                    tr.Participants.Select(p => p.User.TennisLevel = _repoWrapper.TennisLevel.GetFirstAsync(l => l.Id == p.User.TennisLevelId).Result);
                    return new TrainingDTO
                    {
                        Training = _mapper.Map<Training, TrainingInfoDTO>(tr),
                        IsUserTrainingOwner = tr.OwnerId.Equals(userId),
                        IsUserParticipant = tr.Participants == null ? false : tr.Participants.Any(p => p.UserId == userId),
                        IsUserApprovedParticipant = tr.Participants == null ? false : tr.Participants.Any(p => p.UserId == userId && p.ParticipantStatusId == approvedStatus),
                        IsUserUndeterminedParticipant = tr.Participants == null ? false : tr.Participants.Any(p => p.UserId == userId && p.ParticipantStatusId == undeterminedStatus) ,
                        IsUserRejectedParticipant = tr.Participants == null ? false : tr.Participants.Any(p => p.UserId == userId && p.ParticipantStatusId == rejectedStatus)
                    };
                })
                .ToList();
        }
    }
}
