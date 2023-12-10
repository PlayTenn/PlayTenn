using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;
using PlayTen.BLL.Interfaces.EmailSending;
using PlayTen.DAL.Entities;
using PlayTen.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Policy;
using System.Threading.Tasks;

namespace PlayTen.BLL.Services
{
    public class TournamentActionManager: ITournamentActionManager
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IParticipantStatusManager _participantStatusManager;
        private readonly IParticipantManager _participantManager;
        private readonly IEmailContentService _emailContentService;
        private readonly IEmailSendingService _emailSendingService;

        public bool isUserTournamentOwner;
        public bool isUserParticipant;
        public bool isUserApprovedParticipant;
        public bool isUserUndeterminedParticipant;
        public bool isUserRejectedParticipant;

        public TournamentActionManager(UserManager<User> userManager, IRepositoryWrapper repoWrapper, IMapper mapper,
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
        public async Task<IEnumerable<TournamentDTO>> GetTournamentsAsync(User user)
        {
            var tournaments = await _repoWrapper.Tournament.GetAllAsync(include:
                    source => source
                        .Include(e => e.Participants)
                        .ThenInclude(p => p.User)
                        .Include(e => e.Participants)
                        .ThenInclude(p => p.ParticipantStatus)
                        .Include(a => a.Place));

            return await GetTournamentDtosAsync(tournaments, user);
        }

        /// <inheritdoc />
        public async Task<TournamentDTO> GetTournamentInfoAsync(int id, User user)
        {
            int approvedStatus = await _participantStatusManager.GetStatusIdAsync("Прийнято");
            int undeterminedStatus = await _participantStatusManager.GetStatusIdAsync("Розглядається");
            int rejectedStatus = await _participantStatusManager.GetStatusIdAsync("Відмовлено");
            var userRoles = await _userManager.GetRolesAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);


            var targetTraining = await _repoWrapper.Tournament
                .GetFirstAsync(
                    e => e.Id == id,
                    source => source
                        .Include(e => e.Participants)
                        .ThenInclude(p => p.User)
                        .Include(e => e.Participants)
                        .ThenInclude(p => p.ParticipantStatus)
                        .Include(a => a.Place)
                );

            isUserTournamentOwner = targetTraining.OwnerId.Equals(userId);
            if (targetTraining.Participants != null)
            {
                isUserParticipant = targetTraining.Participants.Any(p => p.UserId == userId);
                isUserApprovedParticipant = targetTraining.Participants.Any(p => p.UserId == userId && p.ParticipantStatusId == approvedStatus);
                isUserUndeterminedParticipant = targetTraining.Participants.Any(p => p.UserId == userId && p.ParticipantStatusId == undeterminedStatus);
                isUserRejectedParticipant = targetTraining.Participants.Any(p => p.UserId == userId && p.ParticipantStatusId == rejectedStatus);
            }

            var dto = new TournamentDTO()
            {
                Tournament = _mapper.Map<Tournament, TournamentInfoDTO>(targetTraining),
                IsUserTournamentOwner = isUserTournamentOwner,
                IsUserParticipant = isUserParticipant,
                IsUserApprovedParticipant = isUserApprovedParticipant,
                IsUserUndeterminedParticipant = isUserUndeterminedParticipant,
                IsUserRejectedParticipant = isUserRejectedParticipant,
                IsRegistrationFinished = targetTraining.DateStart < DateTime.Now
            };

            if (!dto.IsUserTournamentOwner)
            {
                dto.Tournament.Participants = dto.Tournament.Participants.Where(p => p.StatusId == approvedStatus);
            }

            return dto;
        }

        /// <inheritdoc />
        public async Task<int> DeleteTournamentAsync(int id)
        {
            try
            {
                Tournament objectToDelete = await _repoWrapper.Tournament
                    .GetFirstAsync(e => e.Id == id);
                _repoWrapper.Tournament.Delete(objectToDelete);
                await _repoWrapper.SaveAsync();
                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<int> SubscribeOnTournamentAsync(int id, User user)
        {
            try
                {
                Tournament targetTraining = await _repoWrapper.Tournament
                    .GetFirstAsync(e => e.Id == id);
                var userId = await _userManager.GetUserIdAsync(user);
                int result = await _participantManager.SubscribeOnTournamentAsync(targetTraining, userId);

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
        public async Task<int> UnSubscribeOnTournamentAsync(int id, User user)
        {
            try
            {
                Tournament targetTournament = await _repoWrapper.Tournament
                    .GetFirstAsync(e => e.Id == id);
                var userId = await _userManager.GetUserIdAsync(user);
                int result = await _participantManager.UnSubscribeOnTournamentAsync(targetTournament, userId);
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


        private async Task<List<TournamentDTO>> GetTournamentDtosAsync(IEnumerable<Tournament> Tournaments, User user)
        {
            int approvedStatus = await _participantStatusManager.GetStatusIdAsync("Прийнято");
            int undeterminedStatus = await _participantStatusManager.GetStatusIdAsync("Розглядається");
            int rejectedStatus = await _participantStatusManager.GetStatusIdAsync("Відмовлено");
            var userRoles = await _userManager.GetRolesAsync(user);

            var userId = _userManager.GetUserIdAsync(user).Result;

            return Tournaments
                .Select(tr =>
                {
                    tr.User = _userManager.FindByIdAsync(tr.OwnerId).Result;
                    tr.Participants.Select(p => p.User.TennisLevel = _repoWrapper.TennisLevel.GetFirstAsync(l => l.Id == p.User.TennisLevelId).Result);
                    return new TournamentDTO
                    {
                        Tournament = _mapper.Map<Tournament, TournamentInfoDTO>(tr),
                        IsUserTournamentOwner = tr.OwnerId.Equals(userId),
                        IsUserParticipant = tr.Participants == null ? false : tr.Participants.Any(p => p.UserId == userId),
                        IsUserApprovedParticipant = tr.Participants == null ? false : tr.Participants.Any(p => p.UserId == userId && p.ParticipantStatusId == approvedStatus),
                        IsUserUndeterminedParticipant = tr.Participants == null ? false : tr.Participants.Any(p => p.UserId == userId && p.ParticipantStatusId == undeterminedStatus),
                        IsUserRejectedParticipant = tr.Participants == null ? false : tr.Participants.Any(p => p.UserId == userId && p.ParticipantStatusId == rejectedStatus)
                    };
                })
                .ToList();
        }
    }
}
