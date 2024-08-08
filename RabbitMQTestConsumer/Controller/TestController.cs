using Microsoft.AspNetCore.Mvc;

namespace RabbitMQTestConsumer.Controller;

[ApiController]
[Route("api/[controller]/[action]")]
public class TestController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetAll()
    {        
        return 1;
    }
}