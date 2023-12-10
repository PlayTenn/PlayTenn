using System;
using System.Collections.Generic;
using System.Text;

namespace PlayTen.DAL.Entities
{
    public class Participant
    {
        public int ID { get; set; }
        public int ParticipantStatusId { get; set; }
        public ParticipantStatus ParticipantStatus { get; set; }
        public int? TrainingId { get; set; }
        public Training Training { get; set; }
        public int? TournamentId { get; set; }
        public Tournament Tournament { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
