using PlayTen.DAL.Entities;
using PlayTen.DAL.Interfaces;

namespace PlayTen.DAL.Repositories
{
    public class TennisLevelRepository: RepositoryBase<TennisLevel>, ITennisLevelRepository
    {
        public TennisLevelRepository(PlayTenDbContext dbContext) : base(dbContext)
        {
        }
    }
}
