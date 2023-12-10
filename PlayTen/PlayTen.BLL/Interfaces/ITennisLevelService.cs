using PlayTen.BLL.DTO;
using PlayTen.DAL.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTen.BLL.Interfaces
{
    public interface ITennisLevelService
    {
        Task<IEnumerable<TennisLevel>> GetAllTennisLevelsAsync();
        Task SetUpUserTennisLevelAsync(string userId, int sportKindId);
        Task<TennisLevelDTO> GetTennisLevelAsync(string userId);
    }
}
