using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class PlacesController : ControllerBase
{
    private readonly OpenStreetMapService _osmService;

    public PlacesController(OpenStreetMapService osmService)
    {
        _osmService = osmService;
    }

    [HttpGet("nearby")]
    public async Task<IActionResult> GetNearbyPlaces(double latitude, double longitude, string key, string value, int radius = 5000)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
            {
                return BadRequest("Key and value are required.");
            }

            var places = await _osmService.GetNearbyPlaces(latitude, longitude, key, value, radius);
            return Ok(places);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);

        }
    }
    // POST: api/places/user-input
    [HttpPost("user-input")]
    public async Task<IActionResult> PostUserInput([FromBody] UserInputModel inputModel)
    {
        if (inputModel == null || string.IsNullOrWhiteSpace(inputModel.UserInput))
        {
            return BadRequest(new { error = "user_input is required" });
        }

        try
        {
            // Call the service to get the SBERT matches
            var matches = await _osmService.GetSbertMatches(inputModel.UserInput);
            return Ok(matches);  // Return the matches from Flask API to the client
        }
        catch (HttpRequestException e)
        {
            return StatusCode(500, new { error = $"Error communicating with Flask API: {e.Message}" });
        }
    }

    // Input model to represent the JSON body
    public class UserInputModel
    {
        public string UserInput { get; set; }
    }
}
