using PlatformService.Model;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDBContext _context;

        public PlatformRepo(AppDBContext context)
        {
            _context = context;
        }
        public void CreatePlatform(Platform platform)
        {
            if (platform is  null)
            {
                throw new ArgumentNullException();
            }
            _context.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform GetPlatformByID(int id)
        {
            return _context.Platforms.FirstOrDefault(x => x.ID == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
