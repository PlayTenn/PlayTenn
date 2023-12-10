using PlayTen.BLL.DTO;
using PlayTen.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTen.BLL.Interfaces
{
    public interface ITournamentActionManager
    {
        Task<IEnumerable<TournamentDTO>> GetTournamentsAsync(User user);
        Task<int> ApproveParticipantAsync(int id);
        Task<int> DeleteTournamentAsync(int id);
        Task<TournamentDTO> GetTournamentInfoAsync(int id, User user);
        Task<int> RejectParticipantAsync(int id);
        Task<int> SubscribeOnTournamentAsync(int id, User user);
        Task<int> UnderReviewParticipantAsync(int id);
        Task<int> UnSubscribeOnTournamentAsync(int id, User user);
    }
}