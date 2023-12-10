using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTen.DAL.Entities;
using PlayTen.DAL.Repositories;
using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace PlayTen.BLL.Services
{
    public class MatchService: IMatchService
    {
        private IRepositoryWrapper repository;
        private IMapper mapper;

        public MatchService(IRepositoryWrapper repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task GenerateDrawAsync(int tournamentId)
        {
            var targetTournament = await repository.Tournament
                .GetFirstAsync(e => e.Id == tournamentId, source => source
                        .Include(e => e.Participants)
                        .ThenInclude(p => p.User));

            var random = new Random();
            var participants = targetTournament.Participants;
            var participantCount = participants.Count();
            int totalParticipants = participantCount;
            var totalMatchesCount = participantCount - 1;

            if (!IsPowerOfTwo(participantCount))
            {
                totalParticipants = CalculateNextPowerOfTwo(participantCount);
                int participantsNeeded = totalParticipants - participantCount;
                for (int i = 0; i < participantsNeeded; i++)
                {
                    participants.Add (new Participant { ID = 1, ParticipantStatusId = 4 });
                }
            }

            var shuffledParticipants = participants.OrderBy(x => random.Next()).ToList();

            var matches = new List<Match>();
            for (int i = 0; i < shuffledParticipants.Count; i += 2)
            {
                var match = new Match
                {
                    TournamentId = tournamentId,
                    Round = 1,
                    Player1Id = shuffledParticipants[i].UserId,
                    Player2Id = shuffledParticipants[i + 1].UserId
                };
                matches.Add(match);
            }

            for (int round = 2; round <= Log2(totalParticipants); round++)
            {
                int matchesInRound = totalParticipants / (int)Math.Pow(2, round);

                for (int i = 0; i < matchesInRound; i++)
                {
                    matches.Add(new Match
                    {
                        TournamentId = tournamentId,
                        Round = round,
                    });
                }
            }

            foreach (var match in matches)
            {
                await repository.Match.CreateAsync(match);
            }
        }

        public async Task AddMatchAsync(Match match)
        {
            var newMatch = new Match
            {
                TournamentId = match.TournamentId,
                Round = match.Round,
                Player1Id = match.Player1Id,
                Player2Id = match.Player2Id
            };
            await repository.Match.CreateAsync(newMatch);
        }

        public void UpdateMatch(Match match)
        {
            repository.Match.Update(match);
        }

        public async Task<Match[]> GetAllMatches(int tournamentId)
        {
            IEnumerable<Match> allMatches = await repository.Match.GetAllAsync(e => e.TournamentId == tournamentId,
                    source => source
                        .Include(e => e.Player1)
                        .Include(p => p.Player2)
                        .Include(e => e.Winner)
                        .Include(p => p.Looser));

            return allMatches.ToArray();
        }

        private bool IsPowerOfTwo(int number)
        {
            return (number & (number - 1)) == 0 && number != 0;
        }

        private int CalculateNextPowerOfTwo(int number)
        {
            int nextPower = 1;
            while (nextPower < number)
            {
                nextPower *= 2;
            }
            return nextPower;
        }
        private int Log2(int n)
        {
            return (int)(Math.Log(n) / Math.Log(2));
        }
    }
}
