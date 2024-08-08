using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQTestPublisher.Options;
using RabbitMQTestPublisher.Services.Interfaces;

namespace RabbitMQTestPublisher.Services;

public class MessageService : IMessageService
{
    private readonly string _exchange = "RabbitMQTestPublisher.Exchange";

    private readonly Dictionary<Type, string> _routingKey = new()
    {
        { typeof(string), "RabbitMQTestPublisher.String" }
    };

    private readonly ConnectionFactory _connectionFactory;

    public MessageService(IOptions<RabbitSettings> options)
    {
        _connectionFactory = new()
        {
            UserName = options.Value.User,
            Password = options.Value.Password,
            Port = 5672,
            HostName = options.Value.Server,
            VirtualHost = options.Value.VirtualHost
        };

        using var con = _connectionFactory.CreateConnection();

        if (con.IsOpen)
        {
            using var channel = con.CreateModel();
            channel.ExchangeDeclare(
                exchange: _exchange,
                type: ExchangeType.Direct,
                durable: true);
        }
    }
    
    public void PublishMessage<T>(T message)
    {
        using var con = _connectionFactory.CreateConnection();
        if (con.IsOpen)
        {
            using var channel = con.CreateModel();
            var body = JsonSerializer.Serialize(message);
            var bytes = Encoding.UTF8.GetBytes(body);
            channel.BasicPublish(
                exchange: _exchange,
                routingKey: _routingKey[message!.GetType()],
                body: bytes);
        }
    }
}