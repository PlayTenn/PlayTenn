using System.Collections.Generic;

namespace PlayTen.DAL.Entities
{
    public class ParticipantStatus
    {
        public int ID { get; set; }
        public string ParticipantStatusName { get; set; }
        public ICollection<Participant> Participants { get; set; }

    }
}
