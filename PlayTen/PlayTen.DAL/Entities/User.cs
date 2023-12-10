using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PlayTen.DAL.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int TennisLevelId { get; set; }
        public string HowToConnect { get; set; }
        public string ProfileImageUrl { get; set; }
        public string ProfileImageFilename { get; set; }
        public DateTime RegistredOn { get; set; }
        public TennisLevel TennisLevel { get; set; }
        public ICollection<Participant> Participants { get; set; }
        public ICollection<Training> Trainings { get; set; }
        public ICollection<Tournament> Tournaments { get; set; }
    }
}
