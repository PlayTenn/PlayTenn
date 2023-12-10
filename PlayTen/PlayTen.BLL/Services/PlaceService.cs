using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTen.DAL.Entities;
using PlayTen.DAL.Repositories;
using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;
using Microsoft.AspNetCore.Http;

namespace PlayTen.BLL.Services
{
    public class PlaceService : IPlaceService
    {
        private const string storageContainerName = "places";
        private IRepositoryWrapper repository;
        private IMapper mapper;
        private IBlobStorageService blobStorageService;

        public PlaceService(IRepositoryWrapper repository, IMapper mapper, IBlobStorageService blobStorageService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.blobStorageService = blobStorageService;
        }

        public async Task<IEnumerable<PlaceDTO>> GetAllAsync()
        {
            var places = await repository.Place.GetAll().ToListAsync();
            //places.ForEach(place => place.Surface = repository.Surface.GetFirstAsync(predicate: x=> x.Id == place.SurfaceId).Result);
            return mapper.Map<List<Place>, List<PlaceDTO>>(places);
        }

        public async Task<PlaceDTO> GetPlaceAsync(int placeId)
        {
            var places = await repository.Place.GetAll(filter: x => x.Id == placeId).FirstAsync();
            return mapper.Map<Place, PlaceDTO>(places);
        }

        public async Task<IEnumerable<PlaceDTO>> GetPlacesBySurfaceIdAsync(int surfaceId)
        {
            var places = await repository.Place.GetAll(filter: x => x.SurfaceId == surfaceId).ToListAsync();
            return mapper.Map<List<Place>, List<PlaceDTO>>(places);
        }

        public async Task AddPlaceAsync(PlaceDTO place)
        {
            var newPlace = mapper.Map<PlaceDTO, Place>(place);
            await repository.Place.CreateAsync(newPlace);
        }

        public void UpdatePlace(PlaceDTO place)
        {
            var newPlace = mapper.Map<PlaceDTO, Place>(place);
            repository.Place.Update(newPlace);
        }

        public void DeletePlace(int placeId)
        {
            var places = repository.Place.GetAll(filter: x => x.Id == placeId).First();
            repository.Place.Delete(places);
        }

        public async Task UploadPhoto(int placeId, IFormFile file)
        {
            PlaceDTO placeDto = await GetPlaceAsync(placeId);
            string oldFilename = placeDto.PhotoFilename;

            BlobDTO blobDto = await blobStorageService.UploadAsync(file, storageContainerName);
            if(!blobDto.Error)
            {
                placeDto.PhotoFilename = blobDto.Name;
                placeDto.PhotoUrl = blobDto.Uri;

                UpdatePlace(placeDto);
                await blobStorageService.DeleteAsync(oldFilename, storageContainerName);
            }            
        }

        public async Task DeletePhoto(int placeId, string filename)
        {
            var response = await blobStorageService.DeleteAsync(filename, storageContainerName);
            if(response == StatusCodes.Status200OK)
            {
                PlaceDTO placeDto = await GetPlaceAsync(placeId);
                placeDto.PhotoFilename = null;
                placeDto.PhotoUrl = null;

                UpdatePlace(placeDto);
            }
            
        }
    }
}
