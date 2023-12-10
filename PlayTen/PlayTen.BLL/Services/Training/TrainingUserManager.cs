using AutoMapper;
using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;
using PlayTen.DAL.Entities;
using PlayTen.DAL.Repositories;
using System;
using System.Threading.Tasks;

namespace PlayTen.BLL.Services
{
    public class TrainingUserManager : ITrainingUserManager
    {
        private readonly IRepositoryWrapper repoWrapper;
        private readonly IMapper mapper;


        public TrainingUserManager(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            this.repoWrapper = repoWrapper;
            this.mapper = mapper;
        }


        public async Task<int> CreateTrainingAsync(TrainingCreateDTO model)
        {
            try
            {
                var trainingToCreate = mapper.Map<TrainingCreateDTO, Training>(model);

                await repoWrapper.Training.CreateAsync(trainingToCreate);
                await repoWrapper.SaveAsync();
                return trainingToCreate.Id;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }

        public async Task EditTrainingAsync(TrainingCreateDTO model)
        {

            var trainingToEdit = mapper.Map<TrainingCreateDTO, Training>(model);
            repoWrapper.Training.Update(trainingToEdit);
            await repoWrapper.SaveAsync();
        }
    }
}
