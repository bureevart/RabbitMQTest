namespace RabbitMQTestPublisher.Services.Interfaces;

public interface IMessageService
{
    public void PublishMessage<T>(T message);
}