using Microsoft.EntityFrameworkCore;
using Onboardle.Data;
using Onboardle.Endpoints.PhotosEndpoints;
using Onboardle.Models;

namespace Onboardle.Endpoints.GamesEndpoints
{
    public static class Games
    {
        public static void RegisterGamesEndpoints(this WebApplication app)
        {
            var games = app.MapGroup("/api/games");

            games.MapGet("/all", async (OnboardleContext _context) =>
            {
                var games = await _context.Games.ToListAsync();
                List<GetGameDto> result = new List<GetGameDto>();

                foreach (var game in games)
                {
                    result.Add(new GetGameDto
                    {
                        GameDate = (DateOnly)game.GameDate!,
                        photoInfo = await _context.Photos.Where(p => p.Id == game.PhotoId).Select(p => new GetPhotoDto
                        {
                            Path = p.Path,
                            Track = p.Track,
                            Driver = p.Driver,
                            Year = p.Year
                        }).FirstOrDefaultAsync()
                    });
                }

                return Results.Ok(result);
            });

            games.MapGet("/", async (OnboardleContext _context) =>
            {
                var game = await _context.Games.FirstOrDefaultAsync(game =>
                    game.GameDate == DateOnly.FromDateTime(DateTime.UtcNow));

                if (game == null)
                {
                    return Results.NotFound();
                }

                var gameDto = new GetGameDto
                {
                    GameDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    photoInfo = await _context.Photos.Where(p => p.Id == game.PhotoId).Select(p => new GetPhotoDto
                    {
                        Path = p.Path,
                        Track = p.Track,
                        Driver = p.Driver,
                        Year = p.Year
                    }).FirstOrDefaultAsync()
                };

                return Results.Ok(gameDto);
            });

            games.MapGet("/getid/{gameDate}", async (OnboardleContext _context, DateOnly gameDate) =>
            {
                var gameId = await _context.Games.Where(g => g.GameDate == gameDate).Select(g => g.Id).FirstOrDefaultAsync();
                return Results.Ok(gameId.ToString());
            });

            games.MapGet("/{gameId}", async (OnboardleContext _context, string gameId) =>
            {
                var game = await _context.Games.FirstOrDefaultAsync(game => game.Id.ToString() == gameId);

                if (game == null)
                {
                    return Results.NotFound();
                }

                var gameDto = new GetGameDto
                {
                    GameDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    photoInfo = await _context.Photos.Where(p => p.Id == game.PhotoId).Select(p => new GetPhotoDto
                    {
                        Path = p.Path,
                        Track = p.Track,
                        Driver = p.Driver,
                        Year = p.Year
                    }).FirstOrDefaultAsync()
                };

                return Results.Ok(gameDto);
            });

            games.MapPost("/", async (OnboardleContext _context, Game game) =>
            {
                await _context.AddAsync(game);
                await _context.SaveChangesAsync();
            });

            games.MapPost("/bulk", async (OnboardleContext _context, List<Game> games) =>
            {
                foreach (var game in games)
                {
                    await _context.AddAsync(game);
                }
                await _context.SaveChangesAsync();
            });

            games.MapDelete("/{id}", async (OnboardleContext _context, Guid id) =>
            {
                _context.Remove(id);
                await _context.SaveChangesAsync();
            });
        }
    }
}
