using Microsoft.EntityFrameworkCore;
using Onboardle.Data;

namespace Onboardle.Endpoints.DriversEndpoints
{
    public static class Drivers
    {
        public static void RegisterDriversEndpoints(this WebApplication app)
        {
            var drivers = app.MapGroup("/api/drivers");

            drivers.MapGet("/{b64Year}", async (OnboardleContext context, string b64Year) =>
            {
                var year = 0;
                var yearString = Convert.FromBase64String(b64Year);

                var convertResult = int.TryParse(yearString, out year);
                if (!convertResult) { return Results.BadRequest(); }

                var driverList = await context.DriverTeamSeasons
                    .Include(dts => dts.Driver)
                    .Include(dts => dts.Team)
                    .Include(dts => dts.Season)
                    .Where(dts => dts.Season.Year == year)
                    .Select(dts => new GetDriverDto
                    {
                        Name = dts.Driver.Name,
                        TeamName = dts.Team.Name
                    })
                    .ToListAsync();

                return Results.Ok(new { drivers = driverList, teams = driverList.Select(dl => dl.TeamName).Distinct().ToList() });
            });
        }
    }
}
