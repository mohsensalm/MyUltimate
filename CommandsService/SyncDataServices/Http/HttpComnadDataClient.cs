using Domain.DTOs;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace CommandsService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpclient;
        private readonly IConfiguration _config;

        public HttpCommandDataClient(HttpClient client, IConfiguration configuration)
        {
            _httpclient = client;
            _config = configuration;
        }

        public async Task<bool> SendPlatformToCommand(PlatformReadDto command)
        {
            var httpcontent = new StringContent(
            JsonSerializer.Serialize(command),
            Encoding.UTF8,
            "application/json");

            var postResponse = await _httpclient.PostAsync("https://localhost:6000/api/c/platform/", httpcontent);
            return postResponse.IsSuccessStatusCode;
        }
    }
}
