using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class OpenStreetMapService
{
//private readonly string _overpassApiUrl = "https://overpass-api.de/api/interpreter";
    private readonly string _overpassApiUrl = "https://maps.mail.ru/osm/tools/overpass/api/interpreter";
    public async Task<string> GetNearbyPlaces(double latitude, double longitude, string key, string value, int radius)
    {
        // Build the Overpass query dynamically based on the key and value
        string overpassQuery = $@"
        [out:json];
        (
          node[""{key}""=""{value}""](around:{radius},{latitude},{longitude});
          way[""{key}""=""{value}""](around:{radius},{latitude},{longitude});
          relation[""{key}""=""{value}""](around:{radius},{latitude},{longitude});
        );
        out body;
        >;
        out skel qt;";

        using (HttpClient client = new HttpClient())
        {
            var content = new StringContent(overpassQuery);
            HttpResponseMessage response = await client.PostAsync(_overpassApiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return ParsePlacesResult(result);
            }
            else
            {
                return "Error retrieving data from Overpass API";
            }
        }
    }

    private string ParsePlacesResult(string jsonResult)
    {
        try
        {
            // Parse the response as a JObject (it may contain multiple fields, including elements array)
            JObject parsedJson = JObject.Parse(jsonResult);

            // Check if the "elements" field exists and is an array
            if (parsedJson["elements"] != null && parsedJson["elements"] is JArray elements)
            {
                string output = "";

                foreach (var place in elements)
                {
                    string name = place["tags"]?["name"]?.ToString() ?? "Unknown place";
                    string lat = place["lat"]?.ToString();
                    string lon = place["lon"]?.ToString();
                    output += $"Name: {name}, Latitude: {lat}, Longitude: {lon}\n";
                }

                return output;
            }
            else
            {
                // If elements array is not found, return an error message or empty result
                return "No places found in the response.";
            }
        }
        catch (Exception ex)
        {
            return $"Error parsing the response: {ex.Message}";
        }
    }

}
