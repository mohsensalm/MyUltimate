using Domain.Model;

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
            _context.Platforms.Add(platform);
            return;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public Platform GetPlatformByID(int id)
        {
            return _context.Platforms.FirstOrDefault(x => x.ID == id);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }
    }
}
