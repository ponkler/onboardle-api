namespace Onboardle.Models
{
    public class DriverTeamSeason
    {
        public int DriverId { get; set; }
        public Driver Driver { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int SeasonId { get; set; }
        public Season Season { get; set; }
    }
}
