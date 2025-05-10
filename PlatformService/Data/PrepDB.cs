namespace PlatformService.Data
{
    public static class PrepDB
    {
        public static void PropPupelition(IApplicationBuilder builder1)
        {
            using (var serviceScope = builder1.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDBContext>());
            }
        }
        private static void SeedData(AppDBContext context)
        {
            if (!context.Platforms.Any())
            {
                Console.WriteLine("sedeing data");
                context.Platforms.AddRange
                    (
                    new Model.Platform() { Name = "dot net", Publisher = "microsoft" },
                    new Model.Platform() { Name = "dot zet", Publisher = "microdone" },
                    new Model.Platform() { Name = "java", Publisher = "unown" }
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
 