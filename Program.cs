// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

HttpClient httpClient = new HttpClient();

OpenSkyService OSS = new OpenSkyService(httpClient);

try
{
    var ukStates = await OSS.GetStatesByBoundingBoxAsync(
    lamin: 35f,
    lamax: 70f,
    lomin: -10,
    lomax: 30f
);
    if (ukStates.Count == 0)
    {
        Console.WriteLine("No flights returned.");
    }
    else
    {
        foreach (var state in ukStates)
        {
            Console.WriteLine($"Callsign: {state.Callsign}, ICAO: {state.Icao24}, Lat: {state.Latitude}, Lon: {state.Longitude}, Altitude: {state.BaroAltitude}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}



