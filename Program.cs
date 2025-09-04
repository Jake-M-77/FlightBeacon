// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

HttpClient httpClient = new HttpClient();

OpenSkyService OSS = new OpenSkyService(httpClient);

string client_id;
string client_secret;

Console.Write("Please enter your Client ID: ");
client_id = Console.ReadLine();
Console.Write("Please enter your Client Secret: ");
client_secret = Console.ReadLine();

await OSS.AuthenticateAsync($"{client_id}", $"{client_secret}");



var departures = await OSS.GetDeparturesByAirportAsync(
    airport: "EDDF",
    begin: 1674993600,
    end: 1674997200
);

if (departures.Count == 0)
{
    Console.WriteLine("No departures found for this period.");
}
else
{
    foreach (var flight in departures)
    {
        Console.WriteLine($"Callsign: {flight.Callsign}, ICAO: {flight.ICao24}, Departure: {flight.estDepartureAirport}, Arrival: {flight.estArrivalAirport}");
    }
}

// try
// {
//     var ukStates = await OSS.GetStatesByBoundingBoxAsync(
//     lamin: 35f,
//     lamax: 70f,
//     lomin: -10,
//     lomax: 30f
// );
//     if (ukStates.Count == 0)
//     {
//         Console.WriteLine("No flights returned.");
//     }
//     else
//     {
//         foreach (var state in ukStates)
//         {
//             Console.WriteLine($"Callsign: {state.Callsign}, ICAO: {state.Icao24}, Lat: {state.Latitude}, Lon: {state.Longitude}, Altitude: {state.BaroAltitude}");
//         }
//     }
// }
// catch (Exception ex)
// {
//     Console.WriteLine($"Error: {ex.Message}");
// }



