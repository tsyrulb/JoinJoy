using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using JoinJoy.Core.Models;
using System;
using System.Threading.Tasks;
using JoinJoy.Core.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace JoinJoy.Infrastructure.Services
{
    public class GISService : IGISService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GISService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<Location> GetCoordinatesAsync(string address)
        {
            var apiKey = _configuration["GoogleMaps:ApiKey"];
            var requestUri = $"https://maps.googleapis.com/maps/api/geocode/json?address={address}&key={apiKey}";

            var response = await _httpClient.GetStringAsync(requestUri);
            var json = JObject.Parse(response);

            var location = json["results"][0]["geometry"]["location"];
            var latitude = (double)location["lat"];
            var longitude = (double)location["lng"];

            return new Location
            {
                Address = address,
                Latitude = latitude,
                Longitude = longitude
            };
        }
    }
}
