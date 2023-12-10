using PlayTen.DAL.Entities;
using PlayTen.DAL.Interfaces;

namespace PlayTen.DAL.Repositories
{
    public class TrainingRepository: RepositoryBase<Training>, ITrainingRepository
    {
        public TrainingRepository(PlayTenDbContext dbContext) : base(dbContext)
        {
        }
    }
}
