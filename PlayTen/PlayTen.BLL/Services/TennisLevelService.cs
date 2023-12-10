using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;
using PlayTen.DAL.Entities;
using PlayTen.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlayTen.BLL.Services
{
    public class TennisLevelService : ITennisLevelService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private const string InvalidUserMessage = "Specified user does not exist.";

        public TennisLevelService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TennisLevel>> GetAllTennisLevelsAsync()
        {
            return await _repository.TennisLevel.GetAll().ToListAsync();
        }

        public async Task<TennisLevelDTO> GetTennisLevelAsync(string userId)
        {
            var user = await _repository.User.GetAll(include: source => source.Include(x => x.TennisLevel),
                filter: s => s.Id == userId).FirstAsync();
            if (user == null)
            {
                throw new ArgumentNullException(InvalidUserMessage);
            }

            return _mapper.Map<TennisLevel, TennisLevelDTO>(user.TennisLevel);
        }

        public async Task SetUpUserTennisLevelAsync(string userId, int tennisLevelId)
        {
            var user = await _repository.User.GetAll(filter: x => x.Id == userId).FirstAsync();
            if (user == null)
            {
                throw new ArgumentNullException(InvalidUserMessage);
            }

            user.TennisLevelId = tennisLevelId;
            _repository.User.Update(user);
        }
    }
}
