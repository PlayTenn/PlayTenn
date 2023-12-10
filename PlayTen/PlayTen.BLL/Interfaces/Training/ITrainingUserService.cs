using PlayTen.BLL.DTO;
using PlayTen.DAL.Entities;
using System.Threading.Tasks;

namespace PlayTen.BLL.Interfaces
{
    public interface ITrainingUserService
    {
        Task<TrainingUserDTO> TrainingUserAsync(string userId, User user);
    }
}