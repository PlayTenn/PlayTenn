using PlayTen.DAL.Entities;
using PlayTen.DAL.Interfaces;

namespace PlayTen.DAL.Repositories
{
    public class PlaceRepository: RepositoryBase<Place>, IPlaceRepository
    {
        public PlaceRepository(PlayTenDbContext dbContext) : base(dbContext)
        {
        }
    }
}
