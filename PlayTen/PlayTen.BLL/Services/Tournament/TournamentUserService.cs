using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;
using PlayTen.DAL.Entities;
using PlayTen.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTen.BLL.Services
{
    public class TournamentUserService: ITournamentUserService
    {
        private readonly IRepositoryWrapper repoWrapper;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IParticipantStatusManager participantStatusManager;
        private readonly IParticipantManager participantManager;


        public TournamentUserService(IRepositoryWrapper repoWrapper, UserManager<User> userManager,
            IParticipantStatusManager participantStatusManager, IMapper mapper, IParticipantManager participantManager)
        {
            this.repoWrapper = repoWrapper;
            this.userManager = userManager;
            this.participantStatusManager = participantStatusManager;
            this.mapper = mapper;
            this.participantManager = participantManager;
        }

        public async Task<TournamentUserDTO> TournamentUserAsync(string userId, User user)
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = await userManager.GetUserIdAsync(user);
            }

            var userWithRoles = await userManager.FindByIdAsync(userId);
            var model = new TournamentUserDTO
            {
                User = mapper.Map<User, UserDTO>(await repoWrapper.User.GetFirstAsync(predicate: q => q.Id == userId))
            };

            var createdTournaments = (await repoWrapper.Tournament.GetAllAsync(predicate: i => i.OwnerId == userId, include: source => source.
                Include(i => i.Participants).Include(i => i.Place).Include(i => i.User))).ToList();

            model.CreatedTournaments = new List<TournamentInfoDTO>();
            foreach (var createdTournament in createdTournaments)
            {
                var tournamentToAdd = mapper.Map<Tournament, TournamentInfoDTO>(createdTournament);
                if (tournamentToAdd.DateEnd > DateTime.Now)
                {
                    model.CreatedTournaments.Add(tournamentToAdd);
                }
            }

            var participants = await participantManager.GetParticipantsByUserIdAsync(userId);
            model.PlanedTournaments = new List<TournamentInfoDTO>();
            model.VisitedTournaments = new List<TournamentInfoDTO>();
            foreach (var participant in participants)
            {
                var TournamentToAdd = mapper.Map<Tournament, TournamentInfoDTO>(participant.Tournament);
                if (participant.Tournament.DateStart >= DateTime.Now)
                {
                    model.PlanedTournaments.Add(TournamentToAdd);
                }
                else if (participant.Tournament.DateStart < DateTime.Now &&
                         participant.ParticipantStatusId == await participantStatusManager.GetStatusIdAsync("Прийнято"))
                {
                    model.VisitedTournaments.Add(TournamentToAdd);
                }
            }
            return model;
        }
    }
}
