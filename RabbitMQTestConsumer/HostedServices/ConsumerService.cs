using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQTestConsumer.Options;

namespace RabbitMQTestConsumer.HostedServices;

public class ConsumerService : BackgroundService
{
    private readonly string _queue = "RabbitMQTestConsumer.Input";
    private readonly string _exchange = "RabbitMQTestPublisher.Exchange";
    private readonly ILogger<ConsumerService> _logger;
    private IModel _channel;
    private IConnection _connection;

    private readonly string[] _routingKeys = new[]
    {
        "RabbitMQTestPublisher.String"
    };

    private readonly ConnectionFactory _connectionFactory;
    
    public ConsumerService(IOptions<RabbitSettings> options, ILogger<ConsumerService> logger)
    {
        _connectionFactory = new()
        {
            UserName = options.Value.User,
            Password = options.Value.Password,
            Port = 5672,
            HostName = options.Value.Server,
            VirtualHost = options.Value.VirtualHost
        };
        _logger = logger;
    }

    private void InitConsumer()
    {
        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(
            queue: _queue,
            exclusive: false,
            durable: true,
            autoDelete: false);
        
        _channel.ExchangeDeclare(
            exchange: _exchange,
            type: ExchangeType.Direct,
            durable: true);

        foreach (var key in _routingKeys)
        {
            _channel.QueueBind(queue: _queue, exchange: _exchange, routingKey: key);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Start background process");

        InitConsumer();
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (sender, args) =>
        {
            _logger.LogInformation("Got new message!");
            var str = Encoding.UTF8.GetString(args.Body.ToArray());
            _channel.BasicAck(args.DeliveryTag, false);
        };
        
        consumer.Shutdown += ConsumerOnShutdown;
        _channel.CallbackException += ChannelOnCallbackException;
        
        _channel.BasicConsume(queue: _queue, autoAck: false, consumer: consumer);
        
        return Task.CompletedTask;
    }

    private void ChannelOnCallbackException(object? sender, CallbackExceptionEventArgs e)
    {
    }

    private void ConsumerOnShutdown(object? sender, ShutdownEventArgs e)
    {
    }
}