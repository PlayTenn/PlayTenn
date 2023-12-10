using PlayTen.BLL.DTO;
using System.Collections.Generic;

namespace PlayTen.BLL.DTO
{
    public class TournamentUserDTO
    {
        public UserDTO User { get; set; }
        public ICollection<TournamentInfoDTO> PlanedTournaments { get; set; }
        public ICollection<TournamentInfoDTO> CreatedTournaments { get; set; }
        public ICollection<TournamentInfoDTO> VisitedTournaments { get; set; }
    }
}