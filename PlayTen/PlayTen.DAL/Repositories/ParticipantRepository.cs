using PlayTen.DAL.Entities;
using PlayTen.DAL.Interfaces;

namespace PlayTen.DAL.Repositories
{
    public class ParticipantRepository: RepositoryBase<Participant>, IParticipantRepository
    {
        public ParticipantRepository(PlayTenDbContext dbContext) : base(dbContext)
        {
        }
    }
}
