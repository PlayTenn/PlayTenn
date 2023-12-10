using PlayTen.DAL.Entities;
using PlayTen.DAL.Interfaces;

namespace PlayTen.DAL.Repositories
{
    public class MatchRepository: RepositoryBase<Match>, IMatchRepository
    {
        public MatchRepository(PlayTenDbContext dbContext) : base(dbContext)
        {
        }
    }
}
