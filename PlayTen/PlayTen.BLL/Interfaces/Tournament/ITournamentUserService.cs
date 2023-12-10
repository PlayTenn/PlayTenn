using PlayTen.BLL.DTO;
using PlayTen.DAL.Entities;
using System.Threading.Tasks;

namespace PlayTen.BLL.Interfaces
{
    public interface ITournamentUserService
    {
        Task<TournamentUserDTO> TournamentUserAsync(string userId, User user);
    }
}