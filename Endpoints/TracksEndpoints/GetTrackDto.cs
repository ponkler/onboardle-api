namespace Onboardle.Endpoints.TracksEndpoints
{
    public class GetTrackDto
    {
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
