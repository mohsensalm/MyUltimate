namespace CommandsService.EventProcessing
{
    public interface IEventProcessor
    {
        EventType ProcessEvent(string message);
    }
}
