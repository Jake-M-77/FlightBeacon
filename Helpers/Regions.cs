public class Regions
{
    public Dictionary<string, BoundingBox> Presets = new Dictionary<string, BoundingBox>
    {
        { "US", new BoundingBox { LatMin = 24.39630f, LatMax = 49.384358f, LonMin = -125.0f, LonMax = -66.93457f } },
        { "UK", new BoundingBox { LatMin = 49.9f, LatMax = 60.85f, LonMin = -8.62f, LonMax = 1.77f } },
        { "EU", new BoundingBox { LatMin = 35.0f, LatMax = 71.0f, LonMin = -10.0f, LonMax = 40.0f } },
        { "Asia", new BoundingBox { LatMin = -10.0f, LatMax = 80.0f, LonMin = 60.0f, LonMax = 150.0f } }
    };
};
