using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class OpenSkyService
{
    private readonly HttpClient _httpClient;

    public OpenSkyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TokenResponse> GetAccessTokenAsync(string clientId, string clientSecret)
    {
        var url = "https://auth.opensky-network.org/auth/realms/opensky-network/protocol/openid-connect/token";

        var formData = new Dictionary<string, string>
        {
            {"grant_type", "client_credentials"},
            {"client_id", clientId},
            {"client_secret", clientSecret}
        };

        using var content = new FormUrlEncodedContent(formData);

        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Failed to get token: {response.StatusCode}");
            return null;
        }

        Console.WriteLine("User Authenticated!");

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TokenResponse>(json);
    }

    private string _accessToken;

    public async Task AuthenticateAsync(string clientId, string clientSecret)
    {
        var tokenResponse = await GetAccessTokenAsync(clientId, clientSecret);
        _accessToken = tokenResponse.access_token;

        //Console.WriteLine($"Token:{tokenResponse.access_token}"); //Debug purposes

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _accessToken);
    }

    public async Task<List<StateVector>> GetStatesByBoundingBoxAsync(float? lamin = null, float? lamax = null, float? lomin = null, float? lomax = null)
    {
        var url = "https://opensky-network.org/api/states/all";

        var query = new List<string>();
        if (lamin.HasValue) query.Add($"lamin={lamin.Value}");
        if (lamax.HasValue) query.Add($"lamax={lamax.Value}");
        if (lomin.HasValue) query.Add($"lomin={lomin.Value}");
        if (lomax.HasValue) query.Add($"lomax={lomax.Value}");

        if (query.Count > 0) url += "?" + string.Join("&", query);

        var response = await _httpClient.GetStringAsync(url);
        Console.WriteLine(url);
        Console.WriteLine(response);

        var json = JsonSerializer.Deserialize<OpenSkyResponse>(response);

        return json?.States ?? new List<StateVector>();
    }

    public async Task<List<FlightData>> GetDeparturesByAirportAsync(string airport, int begin, int end)
    {
        var url = $"https://opensky-network.org/api/flights/departure?airport={airport}&begin={begin}&end={end}";

        try
        {
            var response = await _httpClient.GetStringAsync(url);
            var flights = JsonSerializer.Deserialize<List<FlightData>>(response);
            return flights ?? new List<FlightData>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new List<FlightData>();
        }



    }

    public async Task<List<FlightData>> GetArrivalsByAirportAsync(string airport, int begin, int end)
    {
        var url = $"https://opensky-network.org/api/flights/arrival?airport={airport}&begin={begin}&end={end}";

        try
        {
            var response = await _httpClient.GetStringAsync(url);
            var flights = JsonSerializer.Deserialize<List<FlightData>>(response);
            return flights ?? new List<FlightData>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new List<FlightData>();
        }

        

    }

    public class OpenSkyResponse
    {
        public int Time { get; set; }
        public List<StateVector> States { get; set; }
    }

}