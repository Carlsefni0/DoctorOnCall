using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Azure.Core.GeoJson;
using DoctorOnCall.Controllers;
using DoctorOnCall.DTOs.Route;
using DoctorOnCall.Services.Interfaces;
using NetTopologySuite.Geometries;
using Newtonsoft.Json.Linq;

namespace DoctorOnCall.Services;

public class GoogleMapsService: IGoogleMapsService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GoogleMapsService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["GoogleMaps:ApiKey"];
    }
    //TODO: Review this later
    public async Task<Point> GetCoordinates(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be null or empty", nameof(address));

        string requestUri = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}";

        var response = await _httpClient.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Failed to fetch geolocation data: {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync();

        // Логування вмісту відповіді для налагодження
        Console.WriteLine(content); // або використовуйте ваш логер

        var json = JObject.Parse(content);

        // Перевірка наявності помилки у відповіді API
        var errorMessage = json["error_message"]?.ToString();
        if (!string.IsNullOrEmpty(errorMessage))
            throw new Exception($"Google Maps API error: {errorMessage}");

        // Перевірка наявності результатів
        var results = json["results"] as JArray;
        if (results == null || !results.Any())
            throw new ValidationException("No results found for the given address");

        // Перевірка наявності location у першому результаті
        var firstResult = results[0];
        var geometry = firstResult["geometry"] as JObject;
        var location = geometry?["location"] as JObject;

        if (location == null)
            throw new ValidationException("Location could not be found");

        // Отримання координат
        if (!location.TryGetValue("lat", out var latToken) || !location.TryGetValue("lng", out var lngToken))
            throw new ValidationException("Invalid location data");

        double lat = (double)latToken;
        double lng = (double)lngToken;

        var coordinates = new Point(lat, lng) { SRID = 4326 };
        

        return coordinates;
    }
    public async Task<RouteInfoDto> GetRouteInfo(Point origin, Point destination, string mode)
    {
        if (origin == null || destination == null)
            throw new ArgumentException("Origin and destination points cannot be null");

        string requestUri = $"https://maps.googleapis.com/maps/api/distancematrix/json?" +
                            $"origins={origin.X.ToString(CultureInfo.InvariantCulture)},{origin.Y.ToString(CultureInfo.InvariantCulture)}&" +
                            $"destinations={destination.X.ToString(CultureInfo.InvariantCulture)},{destination.Y.ToString(CultureInfo.InvariantCulture)}&" +
                            $"mode={mode}&key={_apiKey}";

        var response = await _httpClient.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode)
            throw new ApplicationException($"Failed to fetch route data: {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync();
        var json = JObject.Parse(content);

        var element = json["rows"]?[0]?["elements"]?[0];

        if (element == null || element["status"]?.ToString() != "OK")
        {
            throw new ApplicationException($"Invalid response for route calculation. Response: {content}");
        }

        var distanceText = (string)element["distance"]?["text"];
        var distanceValue = (double)element["distance"]?["value"];

        var distance = new Distance() { text = distanceText, value = distanceValue };

        var durationText = (string)element["duration"]?["text"];
        var durationValue = (double)element["duration"]?["value"]; // Тривалість у секундах

        var duration = new Duration
        {
            text = durationText,
            value = durationValue,
            DurationTime = TimeSpan.FromSeconds(durationValue)
        };

        return new RouteInfoDto
        {
            Distance = distance,
            Duration = duration
        };
    }


    
}
