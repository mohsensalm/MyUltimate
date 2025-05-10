using AutoMapper;
using PlatformService.DTOs;
using PlatformService.Model;

namespace PlatformService.Profiles
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            // source => target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();

        }
    }
}
