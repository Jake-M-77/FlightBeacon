using System.Text.Json.Serialization;

public class FlightData
{
    [JsonPropertyName("icao24")]
    public string ICao24 { get; set; }
    public int firstSeen { get; set; }
    public string? estDepartureAirport { get; set; }
    public int lastSeen { get; set; }
    public string? estArrivalAirport { get; set; }
    public string? Callsign { get; set; }
    public int? estDepartureAirportHorizDistance { get; set; }
    public int? estDepartureAirportVertDistance { get; set; }
    public int? estArrivalAirportHorizDistance { get; set; }
    public int? estArrivalAirportVertDistance { get; set; }
    public int? DepartureAirportCandidatesCount { get; set; }
    public int? ArrivalAirportCandidatesCount { get; set; }

}