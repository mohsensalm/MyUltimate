namespace CommandsService.Model
{
    public class Command
    {
        public int Id { get; set; }
        public string HowTo { get; set; } = string.Empty;
        public string CommandLine { get; set; } = string.Empty;
        public int PlatformId { get; set; } 
        public virtual Platform Platform { get; set; } = null!;


        public string GetCommandDetails()
        {
            return $"{HowTo} - {CommandLine} (Platform ID: {PlatformId})";
        }
    }
}
