using MassTransit;
using SharedClass;

namespace RabbitMQTestConsumer.Consumers;

public class RandomClassConsumer : IConsumer<RandomClass>
{
    public Task Consume(ConsumeContext<RandomClass> context)
    {
        Console.WriteLine(context.Message.Message);
        
        return Task.CompletedTask;
    }
}