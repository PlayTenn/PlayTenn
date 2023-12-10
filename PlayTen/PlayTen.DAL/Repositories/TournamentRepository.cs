using PlayTen.DAL.Entities;
using PlayTen.DAL.Interfaces;

namespace PlayTen.DAL.Repositories
{
    public class TournamentRepository: RepositoryBase<Tournament>, ITournamentRepository
    {
        public TournamentRepository(PlayTenDbContext dbContext) : base(dbContext)
        {
        }
    }
}
