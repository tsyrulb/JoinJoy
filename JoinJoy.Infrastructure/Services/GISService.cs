using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using JoinJoy.Core.Models;
using System;

namespace JoinJoy.Infrastructure.Services
{
    public class GISService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GISService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<double> CalculateDistanceAsync(Location location1, Location location2)
        {
            // Example implementation using Google Maps API
            var apiKey = _configuration["GoogleMapsApiKey"];
            var requestUri = $"https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins={location1.Latitude},{location1.Longitude}&destinations={location2.Latitude},{location2.Longitude}&key={apiKey}";

            var response = await _httpClient.GetFromJsonAsync<GoogleDistanceMatrixResponse>(requestUri);
            if (response == null || response.Rows.Count == 0 || response.Rows[0].Elements.Count == 0)
            {
                throw new Exception("Unable to calculate distance");
            }

            return response.Rows[0].Elements[0].Distance.Value / 1000.0; // Convert to kilometers
        }

        private class GoogleDistanceMatrixResponse
        {
            public List<Row> Rows { get; set; }
        }

        private class Row
        {
            public List<Element> Elements { get; set; }
        }

        private class Element
        {
            public Distance Distance { get; set; }
        }

        private class Distance
        {
            public double Value { get; set; }
        }
    }
}
