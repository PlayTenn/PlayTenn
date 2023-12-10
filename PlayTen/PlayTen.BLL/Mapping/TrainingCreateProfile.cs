using PlayTen.BLL.DTO;
using PlayTen.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace PlayTen.BLL.Mapping
{
    public class TrainingCreateProfile : Profile
    {
        public TrainingCreateProfile()
        {
            CreateMap<Training, TrainingCreateDTO>().ReverseMap();
        }
    }
}
