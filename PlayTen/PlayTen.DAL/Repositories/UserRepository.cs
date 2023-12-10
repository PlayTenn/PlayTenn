using PlayTen.DAL.Entities;
using PlayTen.DAL.Interfaces;

namespace PlayTen.DAL.Repositories
{
    public class UserRepository: RepositoryBase<User>, IUserRepository
    {
        public UserRepository(PlayTenDbContext dbContext) : base(dbContext)
        {
        }
    }
}
