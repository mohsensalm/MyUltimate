using CommandsService.AsyncDataServices;
using CommandsService.SyncDataServices.Http;
using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>(); // Add MessageBusClient as a service>

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddHostedService<MessageBusSubscriber>(); // Register the message bus subscriber as a hosted service

// Configure HTTP client for command data service
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
    });

// Enable OpenAPI
builder.Services.AddOpenApi();

// Configure Kestrel server options
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8080); // Enable HTTP on port 8080
    serverOptions.ListenAnyIP(7245, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

// Log Command Service Endpoint
Console.WriteLine($"--> Command Service Endpoint: {builder.Configuration["CommandsService"]};");

// Configure DbContext based on environment
//if (builder.Environment.IsDevelopment())
//{
//    // Use in-memory database for development
//    builder.Services.AddDbContext<AppDBContext>(opt => opt.UseInMemoryDatabase("InMemory"));
//}
//else
//{
    // Use SQL Server for production
    Console.WriteLine("---> using SQL Server db");
    builder.Services.AddDbContext<AppDBContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformConnection")));
//}

var app = builder.Build();

// Prepare the database if needed
// PrepDB.PropPupelition(app, app.Environment.IsProduction());

// Configure the HTTP request pipeline
app.UseAuthorization();
app.MapControllers();
app.MapOpenApi();

app.Run();
