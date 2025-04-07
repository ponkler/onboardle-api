using Onboardle.Endpoints.PhotosEndpoints;

namespace Onboardle.Endpoints.GamesEndpoints
{
    public class GetGameDto
    {
        public DateOnly GameDate { get; set; }
        public GetPhotoDto? photoInfo { get; set; }
    }
}
