using PlayTen.BLL.DTO;
using System.Threading.Tasks;

namespace PlayTen.BLL.Interfaces
{
    public interface ITrainingUserManager
    {
        Task<int> CreateTrainingAsync(TrainingCreateDTO model);
        Task EditTrainingAsync(TrainingCreateDTO model);
    }
}