using PlayTen.DAL.Entities;
using System;
using System.Collections.Generic;

namespace PlayTen.BLL.DTO
{
    public class TournamentInfoDTO
    {
        public int TournamentId { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public DateTime DateStart { get; set; }
        public string Description { get; set; }
        public DateTime DateEnd { get; set; }
        public bool HasStarted { get; set; }
        public bool Finished { get; set; }
        public int AmountOfRounds { get; set; }
        public int TournamentPlaceId { get; set; }
        public int NumberOfParticipants { get; set; }
        public string UserId { get; set; }
        public Place Place { get; set; }
        public string Price { get; set; }
        public IEnumerable<ParticipantDTO> Participants { get; set; }
    }
}
