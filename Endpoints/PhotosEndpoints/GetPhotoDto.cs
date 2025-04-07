namespace Onboardle.Endpoints.PhotosEndpoints
{
    public class GetPhotoDto
    {
        public string Path { get; set; } = string.Empty;
        public string Track { get; set; } = string.Empty;
        public string Driver { get; set; } = string.Empty;
        public int Year { get; set; }
    }
}
