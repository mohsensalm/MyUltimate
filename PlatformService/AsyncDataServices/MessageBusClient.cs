using Domain.DTOs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient, IDisposable
    {
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private IChannel? _channel;
        private bool _disposed = false;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var host = _configuration["RabbitMQHost"];
            var portStr = _configuration["RabbitMQPort"];

            if (string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentException("RabbitMQHost is not configured properly.");
            }

            if (!int.TryParse(portStr, out int port))
            {
                throw new ArgumentException("RabbitMQPort is not a valid integer.");
            }

            var factory = new ConnectionFactory()
            {
                HostName = host,
                Port = port
            };

            try
            {
                InitializeConnection(factory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not connect to the Message Bus: {ex.Message}");
            }
        }

        private async void InitializeConnection(ConnectionFactory factory)
        {
            try
            {
                _connection = await factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();

                await _channel.ExchangeDeclareAsync("trigger", ExchangeType.Fanout);

                _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to MessageBus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not connect to the Message Bus: {ex.Message}");
            }
        }

        public async void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);

            if (_connection != null && _connection.IsOpen && _channel != null)
            {
                Console.WriteLine("--> Sending Message...");
                await SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ connection not open, not sending");
            }
        }

        private async Task SendMessage(string message)
        {
            if (_channel == null) return;

            var body = Encoding.UTF8.GetBytes(message);

            await _channel.BasicPublishAsync(
                exchange: "trigger",
                routingKey: string.Empty,
                body: body);

            Console.WriteLine($"--> We have sent {message}");
        }

        private async Task RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }

        public void Dispose()
        {
            Console.WriteLine("--> MessageBus Disposed");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                Console.WriteLine("--> MessageBus Disposed");

                if (_channel != null)
                {
                    if (_channel.IsOpen)
                    {
                        _channel.CloseAsync().Wait();
                    }
                    _channel.Dispose();
                }

                if (_connection != null)
                {
                    if (_connection.IsOpen)
                    {
                        _connection.CloseAsync().Wait();
                    }
                    _connection.Dispose();
                }

                _disposed = true;
            }
        }
    }
}