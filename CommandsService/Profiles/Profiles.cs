using AutoMapper;
using CommandsService.Model;
using Domain.DTOs;

namespace CommandsService.Profiles
{
    public class CommandProfile : Profile
            {
        public CommandProfile()
        {
            // source => target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, ComandReadDto>();
        }
    }
}
