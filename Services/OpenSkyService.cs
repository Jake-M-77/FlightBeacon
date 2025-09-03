using System.Text.Json;

public class OpenSkyService
{
    private readonly HttpClient _httpClient;

    public OpenSkyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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


    public class OpenSkyResponse
    {
        public int Time { get; set; }
        public List<StateVector> States { get; set; }
    }

}