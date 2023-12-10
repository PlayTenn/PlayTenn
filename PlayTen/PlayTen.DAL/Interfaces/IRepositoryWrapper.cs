using System.Threading.Tasks;
using PlayTen.DAL.Interfaces;

namespace PlayTen.DAL.Repositories
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IParticipantRepository Participant { get; }
        IParticipantStatusRepository ParticipantStatus { get; }
        IPlaceRepository Place { get; }
        ISurfaceRepository Surface { get; }
        ITennisLevelRepository TennisLevel { get; }
        ITournamentRepository Tournament { get; }
        ITrainingRepository Training { get; }
        IMatchRepository Match { get; }
        Task SaveAsync();
    }
}
