
using CommandsService.EventProcessing;
using RabbitMQ.Client;

namespace CommandsService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configration;
        private readonly IEventProcessor _eventProcesseor;
        private Task<IConnection> _connetion;
        private Task<IChannel> _channel;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            _configration = configuration;
            _eventProcesseor = eventProcessor;
        }
        private void initializerabiteMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configration["rabbitMq:HostName"],
                Port = int.Parse(_configration["rabbitMq:Port"])
            };
            _connetion = factory.CreateConnectionAsync();
            _channel = _connetion.GetAwaiter().GetResult().CreateChannelAsync   ()      ;
            _channel.QueueBind()
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
