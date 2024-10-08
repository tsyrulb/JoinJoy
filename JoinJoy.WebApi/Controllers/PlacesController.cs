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

    // POST: api/places/nearby-all
    [HttpPost("nearby-all")]
    public async Task<IActionResult> GetAllNearbyPlaces([FromBody] UserLocationRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.UserInput))
        {
            return BadRequest(new { error = "User input and location coordinates are required." });
        }

        try
        {
            // Call the service to find all nearby places for the given user input
            var nearbyPlaces = await _osmService.GetAllNearbyPlaces(request.Latitude, request.Longitude, request.UserInput, request.Radius);

            if (nearbyPlaces == null || !nearbyPlaces.Any())
            {
                return NotFound(new { message = "No nearby places found." });
            }

            // Return the combined list of all nearby places
            return Ok(nearbyPlaces);
        }
        catch (HttpRequestException e)
        {
            return StatusCode(500, new { error = $"Error communicating with Overpass API: {e.Message}" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
        }
    }
}

public class UserInputModel
{
    public string UserInput { get; set; }
}

// Request model for user input and location coordinates
public class UserLocationRequest
{
    public string UserInput { get; set; } // User's search input
    public double Latitude { get; set; }  // Latitude of the user's location
    public double Longitude { get; set; } // Longitude of the user's location
    public int Radius { get; set; } = 5000; // Optional: Search radius (default 5000 meters)
}