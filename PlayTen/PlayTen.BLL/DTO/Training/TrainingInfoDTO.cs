using PlayTen.DAL.Entities;
using System;
using System.Collections.Generic;

namespace PlayTen.BLL.DTO
{
    public class TrainingInfoDTO
    {
        public int TrainingId { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public DateTime DateStart { get; set; }
        public string Description { get; set; }
        public DateTime DateEnd { get; set; }
        public int TrainingPlaceId { get; set; }
        public int NumberOfParticipants { get; set; }
        public bool HasBalls { get; set; }
        public string UserId { get; set; }
        public Place Place { get; set; }
        public IEnumerable<ParticipantDTO> Participants { get; set; }
    }
}
