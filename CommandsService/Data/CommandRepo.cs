using CommandsService.Model;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }


        public void AddCommand(Command command)
        {
            _context.Commands.Add(command);
            SaveChanges();
        }

        public Command GetCommandById(int id)
        {
            return _context.Commands.FirstOrDefault(c => c.Id == id);
        }
        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands.Where(c => c.PlatformId == platformId).OrderBy(c => c.Platform.Name);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
            return _context.Platforms.FirstOrDefault(p => p.Id == id);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform), "Platform cannot be null");
            }
            _context.Platforms.Add(platform);
            SaveChanges();
        }

        public bool PlatformExists(int platformId)
        {
            // Implementation to check if a platform exists
            return _context.Platforms.Any(p => p.Id == platformId);
        }


        public bool SaveChanges()
        {
            // Implementation for saving changes to the database
            return _context.SaveChanges() >= 0;
        }
        public void CreateCommand(int platformId, Command command)
        {
            // Implementation for creating a command associated with a platform
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "Command cannot be null");
            }
            command.PlatformId = platformId;
            _context.Commands.Add(command);
            SaveChanges();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _context.Commands.Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefault();
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return _context.Commands.ToList();
        }
    }
}


