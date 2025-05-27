
using CommandsService.Data;
using CommandsService.EventProcessing;
using CommandsService.SyncDataServices.Http;
using Microsoft.EntityFrameworkCore;

namespace CommandsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // Add services to the container.

            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
            builder.Services.AddScoped<ICommandRepo, CommandRepo>();
            builder.Services.AddSingleton<IEventProcessor, EventProcessor>();

            builder.Services.AddControllers();
            //builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>().ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            //{
            //    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            //});

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var client = new HttpClient(handler);


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
