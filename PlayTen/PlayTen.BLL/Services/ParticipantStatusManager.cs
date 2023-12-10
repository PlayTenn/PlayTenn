using PlayTen.DAL.Repositories;
using PlayTen.BLL.Interfaces;
using System.Threading.Tasks;

namespace PlayTen.BLL.Services
{
    public class ParticipantStatusManager : IParticipantStatusManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public ParticipantStatusManager(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        /// <inheritdoc />
        public async Task<int> GetStatusIdAsync(string statusName)
        {
            var status = await _repoWrapper.ParticipantStatus
                .GetFirstAsync(predicate: p => p.ParticipantStatusName == statusName);

            return status.ID;
        }
    }
}
