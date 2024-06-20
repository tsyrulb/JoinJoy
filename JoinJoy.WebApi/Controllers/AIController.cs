using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class AIController : ControllerBase
{
    private readonly FlaskApiService _flaskApiService;

    public AIController(FlaskApiService flaskApiService)
    {
        _flaskApiService = flaskApiService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateText([FromBody] UserInputRequest request)
    {
        var result = await _flaskApiService.GetGeneratedTextAsync(request.Input);
        return Ok(result);
    }
}

public class UserInputRequest
{
    public string Input { get; set; }
}
