using PlatformService.Model;

namespace PlatformService.Data
{
    public interface IPlatformRepo
    {
        bool SaveChanges();
        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatformByID(int id);
        void CreatePlatform(Platform platform);
    }
}
