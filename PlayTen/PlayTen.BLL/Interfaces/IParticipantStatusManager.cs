using System.Threading.Tasks;

namespace PlayTen.BLL.Interfaces
{
    public interface IParticipantStatusManager
    {
        Task<int> GetStatusIdAsync(string statusName);
    }
}