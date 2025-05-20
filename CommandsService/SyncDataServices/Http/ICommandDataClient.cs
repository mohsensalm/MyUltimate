
using PlatformService.DTOs;

namespace CommandsService.SyncDataServices.Http
{
    public interface ICommandDataClient
    {
        Task<bool> SendPlatformToCommand(PlatformReadDto command);
    }
}
