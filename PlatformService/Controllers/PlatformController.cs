using AutoMapper;
using CommandsService.SyncDataServices.Http;
using Domain.DTOs;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using System.Threading.Tasks;

namespace PlatformService.Controllers
{
    [Route("api/{Controller}")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _dataClient;

        public PlatformController(IPlatformRepo repo, IMapper mapper, ICommandDataClient dataClient )
        {
            _repository = repo;
            _mapper = mapper;
            _dataClient= dataClient;
        }
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatform()
        {
            Console.WriteLine("geting platforms....");
            var platformItems = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name = nameof(GetPlatformByID))]
        public ActionResult<PlatformReadDto> GetPlatformByID(int id)
        {
            var platformItem = _repository.GetPlatformByID(id);
            if (platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();
            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                await _dataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {

                Console.WriteLine("⚠️ SSL Error: " + ex.InnerException?.Message);
                return StatusCode(500, "Internal server error");
            }

            return CreatedAtRoute(nameof(GetPlatformByID), new { Id = platformReadDto.ID }, platformReadDto);
        }
    }
}
