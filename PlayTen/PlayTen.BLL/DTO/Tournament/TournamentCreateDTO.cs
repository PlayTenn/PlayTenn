using System;
using System.Collections.Generic;
using System.Text;

namespace PlayTen.BLL.DTO
{
    public class TournamentCreateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public DateTime DateStart { get; set; }
        public int PlaceId { get; set; }
        public DateTime DateEnd { get; set; }
        public string OwnerId { get; set; }
        public int NumberOfParticipants { get; set; }
    }
}
