using Microsoft.EntityFrameworkCore;

namespace PlatformService.Data
{
    public static class PrepDB
    {
        public static void PropPupelition(IApplicationBuilder builder1,bool isProd)
        {
            using (var serviceScope = builder1.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDBContext>(), isProd);
            }
        }
        private static void SeedData(AppDBContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("Applying migrations...");
                try
                {
                    context.Database.Migrate();

                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
                }
            }


            if (!context.Platforms.Any())
            {
                Console.WriteLine("sedeing data");
                context.Platforms.AddRange
                    (
                    new Domain.Model.Platform() { Name = "dot net", Publisher = "microsoft" },
                    new Domain.Model.Platform() { Name = "dot zet", Publisher = "microdone" },
                    new Domain.Model.Platform() { Name = "java", Publisher = "unown" }
                    );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("we have data");
            }
        }
    }
}
 