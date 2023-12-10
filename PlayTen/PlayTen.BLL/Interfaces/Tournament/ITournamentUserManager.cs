using PlayTen.BLL.DTO;
using System.Threading.Tasks;

namespace PlayTen.BLL.Interfaces
{
    public interface ITournamentUserManager
    {
        Task<int> CreateTournamentAsync(TournamentCreateDTO model);
        Task EditTournamentAsync(TournamentCreateDTO model);
        Task StartTournament(int id);
        Task FinishTournament(int id);
    }
}