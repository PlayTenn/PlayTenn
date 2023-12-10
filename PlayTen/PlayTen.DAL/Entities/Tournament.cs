using System;
using System.Collections.Generic;
using System.Text;

namespace PlayTen.DAL.Entities
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public bool HasStarted { get; set; }
        public bool Finished { get; set; }
        public int AmountOfRounds { get; set; }
        public int NumberOfParticipants { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public int PlaceId { get; set; }
        public string OwnerId { get; set; }
        public ICollection<Participant> Participants { get; set; }
        public Place Place { get; set; }
        public User User { get; set; }
    }
}
