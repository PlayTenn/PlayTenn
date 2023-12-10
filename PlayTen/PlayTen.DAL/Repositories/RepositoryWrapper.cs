using PlayTen.DAL.Interfaces;
using System.Threading.Tasks;

namespace PlayTen.DAL.Repositories
{
    public class RepositoryWrapper: IRepositoryWrapper
    {
        private readonly PlayTenDbContext _dbContext;
        private IUserRepository _user;
        private ISurfaceRepository _surfaceRepository;
        private IParticipantRepository _participantRepository;
        private IParticipantStatusRepository _participantStatusesRepository;
        private IPlaceRepository _placeRepository;
        private ITennisLevelRepository _tennisLevelRepository;
        private ITournamentRepository _tournamentRepository;
        private ITrainingRepository _trainingRepository;
        private IMatchRepository _matchRepository;

        public RepositoryWrapper(PlayTenDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUserRepository User
        {
            get { return _user ??= new UserRepository(_dbContext); }
        }

        public ISurfaceRepository Surface
        {
            get { return _surfaceRepository ??= new SurfaceRepository(_dbContext); }
        }

        public IParticipantRepository Participant
        {
            get { return _participantRepository ??= new ParticipantRepository(_dbContext); }
        }

        public IParticipantStatusRepository ParticipantStatus
        {
            get { return _participantStatusesRepository ??= new ParticipantStatusRepository(_dbContext); }
        }

        public IPlaceRepository Place
        {
            get { return _placeRepository ??= new PlaceRepository(_dbContext); }
        }

        public ITennisLevelRepository TennisLevel
        {
            get { return _tennisLevelRepository ??= new TennisLevelRepository(_dbContext); }
        }

        public ITournamentRepository Tournament
        {
            get { return _tournamentRepository ??= new TournamentRepository(_dbContext); }
        }

        public ITrainingRepository Training
        {
            get { return _trainingRepository ??= new TrainingRepository(_dbContext); }
        }
        public IMatchRepository Match
        {
            get { return _matchRepository ??= new MatchRepository(_dbContext); }
        }


        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
