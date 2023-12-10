using PlayTen.BLL.DTO;
using PlayTen.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTen.BLL.Interfaces
{
    public interface ITrainingActionManager
    {
        Task<IEnumerable<TrainingDTO>> GetTrainingsAsync(User user);
        Task<int> ApproveParticipantAsync(int id);
        Task<int> DeleteTrainingAsync(int id);
        Task<TrainingDTO> GetTrainingInfoAsync(int id, User user);
        Task<int> RejectParticipantAsync(int id);
        Task<int> SubscribeOnTrainingAsync(int id, User user);
        Task<int> UnderReviewParticipantAsync(int id);
        Task<int> UnSubscribeOnTrainingAsync(int id, User user);
    }
}