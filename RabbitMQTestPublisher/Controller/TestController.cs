using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RabbitMQTestPublisher.Services.Interfaces;

namespace RabbitMQTestPublisher.Controller;

[ApiController]
[Route("api/[controller]/[action]")]
public class TestController
{
    private readonly IMessageService _messageService;
    
    public TestController(IMessageService messageService)
    {
        _messageService = messageService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetAll()
    {
        _messageService.PublishMessage("Hello world");
        
        return 1;
    }
}