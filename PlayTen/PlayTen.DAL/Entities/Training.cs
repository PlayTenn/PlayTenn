using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlayTen.DAL.Entities
{
    public class Training
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int NumberOfParticipants { get; set; }
        public string Description { get; set; }
        public bool HasBalls { get; set; }
        [Required]
        public int PlaceId { get; set; }
        public Place Place { get; set; }
        [Required]
        public string OwnerId { get; set; }
        public User User { get; set; }
        public ICollection<Participant> Participants { get; set; }
        
        
    }
}
