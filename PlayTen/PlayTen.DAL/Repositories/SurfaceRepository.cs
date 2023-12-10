using PlayTen.DAL.Entities;
using PlayTen.DAL.Interfaces;

namespace PlayTen.DAL.Repositories
{
    public class SurfaceRepository : RepositoryBase<Surface>, ISurfaceRepository
    {
        public SurfaceRepository(PlayTenDbContext dbContext) : base(dbContext)
        {
        }
    }
}
