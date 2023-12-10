using PlayTen.DAL.Entities;
using PlayTen.DAL.Interfaces;

namespace PlayTen.DAL.Repositories
{
    public class ParticipantStatusRepository : RepositoryBase<ParticipantStatus>, IParticipantStatusRepository
    {
        public ParticipantStatusRepository(PlayTenDbContext dbContext) : base(dbContext)
        {
        }
    }
}
