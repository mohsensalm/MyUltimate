using CommandsService.Data;
using CommandsService.Model;
using Domain.DTOs;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly AutoMapper.IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, AutoMapper.IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;

        }
        public EventType ProcessEvent(string message)
        {
            var eventType = determineEvent(message);
            switch (eventType)
            { 
                case EventType.PlatformPublished:
                    Console.WriteLine( "publish event detected");
                    return EventType.PlatformPublished;
            default:
                    Console.WriteLine("could not determind");
                    return EventType.undetermined;
            }
        }
        private EventType determineEvent(string eventType)
        {
            var eventype = JsonSerializer.Deserialize<GenericEventDto>(eventType);
            return eventType switch
            {
                "Platform_Published" => EventType.PlatformPublished,
                "CommandPublished" => EventType.CommandPublished,
                "CommandDeleted" => EventType.CommandDeleted,
                _ => EventType.undetermined,
            };
        }
        private void AddPlatform(string platformpublishMessage)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
            var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformpublishMessage);
            try
            {
                var plat = _mapper.Map<Platform>(platformpublishMessage);
                if (!repo.ExternalPlatformExists(plat.ExternalID))
                {
                    repo.CreatePlatform(plat);
                    repo.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Platform already exists");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not add platform to DB {ex.Message}");
            }
        }
    }
   public enum EventType
    {
        PlatformPublished = 1,
        CommandPublished = 2,
        CommandDeleted = 3,
        undetermined = 0
    }
}

