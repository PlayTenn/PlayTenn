using AutoMapper;
using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;
using PlayTen.DAL.Entities;
using PlayTen.DAL.Repositories;
using System;
using System.Threading.Tasks;

namespace PlayTen.BLL.Services
{
    public class TournamentUserManager: ITournamentUserManager
    {
        private readonly IRepositoryWrapper repoWrapper;
        private readonly IMapper mapper;


        public TournamentUserManager(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            this.repoWrapper = repoWrapper;
            this.mapper = mapper;
        }


        public async Task<int> CreateTournamentAsync(TournamentCreateDTO model)
        {
            try
            {
                var tournamentToCreate = mapper.Map<TournamentCreateDTO, Tournament>(model);
                tournamentToCreate.AmountOfRounds = FindExponent(tournamentToCreate.NumberOfParticipants);

                await repoWrapper.Tournament.CreateAsync(tournamentToCreate);
                await repoWrapper.SaveAsync();
                return tournamentToCreate.Id;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }

        public async Task EditTournamentAsync(TournamentCreateDTO model)
        {
            var tournamentToEdit = mapper.Map<TournamentCreateDTO, Tournament>(model);
            repoWrapper.Tournament.Update(tournamentToEdit);
            await repoWrapper.SaveAsync();
        }

        public async Task StartTournament(int id)
        {
            var tournamentToStart = await repoWrapper.Tournament.GetFirstAsync(t => t.Id == id);
            tournamentToStart.HasStarted = true;
            tournamentToStart.Finished = false;

            repoWrapper.Tournament.Update(tournamentToStart);
            await repoWrapper.SaveAsync();
        }

        public async Task FinishTournament(int id)
        {
            var tournamentToStart = await repoWrapper.Tournament.GetFirstAsync(t => t.Id == id);
            tournamentToStart.Finished = true;
            tournamentToStart.HasStarted = false;

            repoWrapper.Tournament.Update(tournamentToStart);
            await repoWrapper.SaveAsync();
        }

        private int FindExponent(int number)
        {
            int exponent = 0;
            while (number > 1)
            {
                number /= 2;
                exponent++;
            }

            return exponent;
        }
    }
}
