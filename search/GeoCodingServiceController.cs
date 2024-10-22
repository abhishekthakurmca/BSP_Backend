using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class GeocodingService
{
    private readonly string _apiKey;
    private readonly RestClient _client;

    public GeocodingService(IConfiguration configuration)
    {
        _apiKey = configuration["GoogleGeocodingApiKey"];
        _client = new RestClient("https://maps.googleapis.com/maps/api/geocode/json");
    }

    public async Task<(decimal? lat, decimal? lng)> GetCoordinatesAsync(string address)
    {
        var request = new RestRequest();
        request.AddParameter("address", address);
        request.AddParameter("key", _apiKey);

        var response = await _client.ExecuteAsync(request);
        if (response.IsSuccessful)
        {
            var jsonResponse = JObject.Parse(response.Content);
            var location = jsonResponse["results"]?[0]?["geometry"]?["location"];

            if (location != null)
            {
                decimal lat = (decimal)location["lat"];
                decimal lng = (decimal)location["lng"];
                return (lat, lng);
            }
        }

        return (null, null); // Return null if failed
    }
}