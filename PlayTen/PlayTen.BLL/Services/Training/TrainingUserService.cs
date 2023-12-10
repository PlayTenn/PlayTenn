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
    public class TrainingUserService : ITrainingUserService
    {
        private readonly IRepositoryWrapper repoWrapper;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IParticipantStatusManager participantStatusManager;
        private readonly IParticipantManager participantManager;


        public TrainingUserService(IRepositoryWrapper repoWrapper, UserManager<User> userManager,
            IParticipantStatusManager participantStatusManager, IMapper mapper, IParticipantManager participantManager)
        {
            this.repoWrapper = repoWrapper;
            this.userManager = userManager;
            this.participantStatusManager = participantStatusManager;
            this.mapper = mapper;
            this.participantManager = participantManager;
        }

        public async Task<TrainingUserDTO> TrainingUserAsync(string userId, User user)
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = await userManager.GetUserIdAsync(user);
            }

            var userWithRoles = await userManager.FindByIdAsync(userId);
            var model = new TrainingUserDTO
            {
                User = mapper.Map<User, UserDTO>(await repoWrapper.User.GetFirstAsync(predicate: q => q.Id == userId))
            };

            var createdTrainings = (await repoWrapper.Training.GetAllAsync(predicate: i => i.OwnerId == userId, include: source => source.
                Include(i => i.Participants).Include(i => i.Place).Include(i => i.User))).ToList();

            model.CreatedTrainings = new List<TrainingInfoDTO>();
            foreach (var createdTraining in createdTrainings)
            {
                var trainingToAdd = mapper.Map<Training, TrainingInfoDTO>(createdTraining);
                if (trainingToAdd.DateStart > DateTime.Now)
                {
                    model.CreatedTrainings.Add(trainingToAdd);
                }
            }

            var participants = await participantManager.GetParticipantsByUserIdAsync(userId);
            model.PlanedTrainings = new List<TrainingInfoDTO>();
            model.VisitedTrainings = new List<TrainingInfoDTO>();
            foreach (var participant in participants)
            {
                var TrainingToAdd = mapper.Map<Training, TrainingInfoDTO>(participant.Training);
                if (participant.Training.DateStart >= DateTime.Now)
                {
                    model.PlanedTrainings.Add(TrainingToAdd);
                }
                else if (participant.Training.DateStart < DateTime.Now &&
                         participant.ParticipantStatusId == await participantStatusManager.GetStatusIdAsync("Прийнято"))
                {
                    model.VisitedTrainings.Add(TrainingToAdd);
                }
            }
            return model;
        }
    }
}
