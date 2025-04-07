namespace Onboardle.Models
{
    public class TrackSeason
    {
        public int TrackId { get; set; }
        public Track Track { get; set; }

        public int SeasonId { get; set; }
        public Season Season { get; set; }
    }
}
