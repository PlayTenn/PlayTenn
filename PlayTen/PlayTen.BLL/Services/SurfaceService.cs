using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTen.DAL.Entities;
using PlayTen.DAL.Repositories;
using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;

namespace PlayTen.BLL.Services
{
    public class SurfaceService : ISurfaceService
    {
        private IRepositoryWrapper repository;
        private IMapper mapper;

        public SurfaceService(IRepositoryWrapper repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SurfaceDTO>> GetAllAsync()
        {
            var surfaces = await repository.Surface.GetAll().ToListAsync();
            return mapper.Map<List<Surface>, List<SurfaceDTO>>(surfaces);
        }

        public async Task<SurfaceDTO> GetSurfaceAsync(int surfaceId)
        {
            var surface = await repository.Surface.GetAll(filter: x => x.Id == surfaceId).FirstAsync();
            return mapper.Map<Surface, SurfaceDTO>(surface);
        }
    }
}
