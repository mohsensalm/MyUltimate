using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace PlatformService.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
                
        }
        public DbSet<Platform> Platforms { get; set; }
    }
}
