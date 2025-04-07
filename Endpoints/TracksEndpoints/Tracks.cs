using Microsoft.EntityFrameworkCore;
using Onboardle.Data;
using Onboardle.Models;

namespace Onboardle.Endpoints.TracksEndpoints
{
    public static class Tracks
    {
        public static void RegisterTracksEndpoints(this WebApplication app)
        {
            var tracks = app.MapGroup("/api/tracks");

            // GET tracks that were raced at in the given year
            tracks.MapGet("/{b64Year}", async (OnboardleContext context, string b64Year) =>
            {
                var year = 0;
                var yearString = Convert.FromBase64String(b64Year);

                var convertResult = int.TryParse(yearString, out year);
                if (!convertResult) { return Results.BadRequest(); }

                var trackList = await context.TrackSeasons
                    .Include(ts => ts.Track)
                    .Include(ts => ts.Season)
                    .Where(ts => ts.Season.Year == year)
                    .Select(ts => new GetTrackDto
                    {
                        Name = ts.Track.Name,
                        Country = ts.Track.Country,
                        Latitude = ts.Track.Latitude,
                        Longitude = ts.Track.Longitude
                    })
                    .ToListAsync();

                return Results.Ok(new { tracks = trackList, countries = trackList.Select(t => t.Country).Distinct().ToList() });
            });

            tracks.MapGet("/", async (OnboardleContext _context) =>
            {
                var tracks = await _context.Tracks
                    .ToListAsync();

                return Results.Ok(new { tracks = tracks });
            });
        }
    }
}
