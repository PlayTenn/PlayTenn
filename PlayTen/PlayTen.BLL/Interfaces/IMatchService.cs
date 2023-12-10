using PlayTen.DAL.Entities;
using System.Threading.Tasks;

namespace PlayTen.BLL.Services
{
    public interface IMatchService
    {
        Task GenerateDrawAsync(int tournamentId);
        Task<Match[]> GetAllMatches(int tournamentId);
        Task AddMatchAsync(Match match);
        void UpdateMatch(Match match);
    }
}