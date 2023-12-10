﻿using AutoMapper;
using PlayTen.DAL.Entities;
using PlayTen.BLL.DTO;

namespace PlayTen.BLL.Mapping
{
    public class PlaceProfile: Profile
    {
        public PlaceProfile()
        {
            CreateMap<Place, PlaceDTO>().ReverseMap();
        }
    }
}
