using Microsoft.AspNetCore.Mvc;

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
}
