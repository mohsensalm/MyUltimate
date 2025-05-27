using CommandsService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CommandsService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IChannel _channel;
        private string _queueName;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            InitializeRabbitMQ().GetAwaiter().GetResult();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            await InitializeRabbitMQ();

            stoppingToken.Register(() =>
            {
                Console.WriteLine("RabbitMQ connection is shutting down...");
            });

            // Keep the service running
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:Host"],
                Port = int.Parse(_configuration["RabbitMQ:Port"])
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(exchange: "trigger", type: ExchangeType.Fanout);

            // Create a temporary queue
            var queueDeclareResult = await _channel.QueueDeclareAsync();
            _queueName = queueDeclareResult.QueueName;

            await _channel.QueueBindAsync(queue: _queueName, exchange: "trigger", routingKey: "");

            Console.WriteLine("Subscribed to the message bus");

            _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdown;

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += (ModuleHandle, ea) =>
            {
                Console.WriteLine("Event received!");

                var body = ea.Body.ToArray();
                var notificationMessage = Encoding.UTF8.GetString(body);

                _eventProcessor.ProcessEvent(notificationMessage);

                return Task.CompletedTask;
            };

            await _channel.BasicConsumeAsync(_queueName, autoAck: true, consumer);
        }

        private Task RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("RabbitMQ connection shutdown");
            return Task.CompletedTask;
        }

        public override async void Dispose()
        {
            if (_channel?.IsOpen == true)
            {
                await _channel.CloseAsync();
            }

            if (_connection?.IsOpen == true)
            {
                await _connection.CloseAsync();
            }

            base.Dispose();
        }
    }
}