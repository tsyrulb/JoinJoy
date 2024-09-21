using Newtonsoft.Json;

public class GeocodeResponse
{
    [JsonProperty("place_id")]
    public long PlaceId { get; set; }

    [JsonProperty("lat")]
    public string Lat { get; set; }

    [JsonProperty("lon")]
    public string Lon { get; set; }

    [JsonProperty("display_name")]
    public string DisplayName { get; set; }

    // Additional properties if needed can be added here
}
