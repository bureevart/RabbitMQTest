using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RabbitMQTestPublisher.Services.Interfaces;
using SharedClass;

namespace RabbitMQTestPublisher.Controller;

[ApiController]
[Route("api/[controller]/[action]")]
public class TestController
{
    private readonly IMessageService _messageService;
    private readonly IPublishEndpoint _publishEndpoint;
    
    public TestController(IMessageService messageService, IPublishEndpoint publishEndpoint)
    {
        _messageService = messageService;
        _publishEndpoint = publishEndpoint;
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> Send(RandomClass randomClass)
    {
        //_messageService.PublishMessage("Hello world");
        await _publishEndpoint.Publish<RandomClass>(randomClass);
        return 1;
    }
}