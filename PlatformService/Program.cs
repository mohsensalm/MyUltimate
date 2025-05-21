using CommandsService.SyncDataServices.Http;
using Microsoft.EntityFrameworkCore;
using PlatformService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddDbContext<AppDBContext>(opt => opt.UseInMemoryDatabase("InMemory"));


builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>()
    .ConfigurePrimaryHttpMessageHandler(static () => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,

    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8080); // Enable HTTP on port 8080
    serverOptions.ListenAnyIP(7245, listenOptions =>
    {
        listenOptions.UseHttps();
    });

});
var handler = new HttpClientHandler();
handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

var client = new HttpClient(handler);


Console.WriteLine($"--> Command Service Endpoint: {builder.Configuration["CommandsService"]};");

var app = builder.Build();

PrepDB.PropPupelition(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
