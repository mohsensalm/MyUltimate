using AutoMapper;
using CommandsService.Data;
using CommandsService.Model;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/Platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public CommandController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{comandId}", Name = "GetCommandForPlatform")]
        public ActionResult<IEnumerable<ComandReadDto>> GetCommandForPlatform(int platformId, int comandId)
        {
            if (!_repository.PlatformExists(platformId))
            {
                return NotFound($"Platform with ID {platformId} not found.");
            }
            var commands = _repository.GetCommand(platformId, comandId);
            if (commands == null)
            {
                return NotFound($"Command with ID {comandId} not found.");
            }

            return Ok(_mapper.Map<IEnumerable<ComandReadDto>>(commands));
        }
        [HttpPost]
        public ActionResult<ComandReadDto> CreateComandForPlatform(int platformId, CommandCreateDto commandCreateDto)
        {
            if (!_repository.PlatformExists(platformId))
            {
                return NotFound($"Platform with ID {platformId} not found.");   
                          
            }
            else
            {
                var commandModel = _mapper.Map<Command>(commandCreateDto);
                _repository.CreateCommand(platformId, commandModel);
                _repository.SaveChanges();
                var commandReadDto = _mapper.Map<ComandReadDto>(commandModel);
                return CreatedAtRoute(nameof(GetCommandForPlatform), new { platformId, commandId = commandReadDto.Id }, commandReadDto);

            }
        }
    }
}
