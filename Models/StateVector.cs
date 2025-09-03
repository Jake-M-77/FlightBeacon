public class StateVector
{
    public string Icao24 { get; set; }
    public string? Callsign { get; set; }
    public string OriginCountry { get; set; }
    public int? TimePosition { get; set; }
    public float? Longitude { get; set; }
    public float? Latitude { get; set; }
    public float? BaroAltitude { get; set; }
    public bool OnGround { get; set; }
    public float? Velocity { get; set; }
    public float? TrueTrack { get; set; }
    public float? VerticalRate { get; set; }
    public int[] Sensors { get; set; }
    public float? GeoAltitude { get; set; }
    public string? Squawk { get; set; }
    public bool Spi { get; set; }
    public PositionSource PositionSource { get; set; }
    public AircraftCategory Category { get; set; }

}


public enum PositionSource
{
    ADSB = 0,
    ASTERIX = 1,
    MLAT = 2,
    FLARM = 3
}

public enum AircraftCategory
{
    NoInformation = 0,
    No_ADSBEmitterCategoryInformation = 1,
    Light = 2,
    Small = 3,
    Large = 4,
    HighVortexLarge = 5,
    Heavy = 6,
    HighPerformance = 7,
    Rotorcraft = 8,
    GliderSailplane = 9,
    LighterThanAir = 10,
    ParachutistSkydiver = 11,
    UltralightHanggliderParaglider = 12,
    Reserved = 13,
    UnmannedAerialVehicle = 14,
    SpaceTransAtmosphericVehicle = 15,
    SurfaceVehicleEmergencyVehicle = 16,
    SurfaceVehicleServiceVehicle = 17,
    PointObstacle = 18,
    ClusterObstacle = 19,
    LineObstacle = 20
}