using AutoMapper;
using NzWalk.API.Models.Domain;
using NzWalk.API.Models.DTO;

namespace NzWalk.API.Mappers
    {
    public class ProfileMappers: Profile
        {
        public ProfileMappers()
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<CreateRegionDto, Region>().ReverseMap();
            CreateMap<UpdateRegionDTO, Region>().ReverseMap();
            CreateMap<AddWalkDTO, Walk>().ReverseMap();
            CreateMap<Walk, WalkDTO>().ReverseMap();
            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();
            CreateMap<UpdateWalkDTO, Walk>().ReverseMap(); 

            }
    }
    }
