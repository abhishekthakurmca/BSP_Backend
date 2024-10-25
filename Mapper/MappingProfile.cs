using AutoMapper;
using MyBackendApp.Dto.Home;
using MyBackendApp.Models;

namespace MyBackendApp.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<HomeUser, HomeUserDto>().ReverseMap();
    }
}
