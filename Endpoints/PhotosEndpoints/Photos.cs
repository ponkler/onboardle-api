using Amazon.S3;
using Amazon.S3.Model;
using Onboardle.Data;
using Onboardle.Models;

namespace Onboardle.Endpoints.PhotosEndpoints
{
    public static class Photos
    {
        public static void RegisterPhotosEndpoints(this WebApplication app, bool isProduction)
        {
            var photos = app.MapGroup("/api/photos");

            if (isProduction)
            {
                photos.MapGet("/{photoFile}", async (string photoFile) =>
                {
                    var s3Client = new AmazonS3Client();
                    var request = new GetObjectRequest { BucketName = "onboardle-photos", Key = photoFile };

                    var response = await s3Client.GetObjectAsync(request);
                    return Results.File(response.ResponseStream, response.Headers["Content-Type"]);
                });
            }
            else
            {
                var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "Photos");

                if (!Directory.Exists(photoPath))
                {
                    throw new DirectoryNotFoundException();
                }

                photos.MapGet("/{photoFile}", (string photoFile) =>
                {
                    var filePath = Path.Combine(photoPath, photoFile);
                    if (!File.Exists(filePath)) {
                        return Results.NotFound();
                    }

                    return Results.File(filePath, "image/jpeg");
                });
            }

            photos.MapPost("/", async (OnboardleContext _context, Photo photo) =>
            {
                await _context.Photos.AddAsync(photo);
                await _context.SaveChangesAsync();
            });

            photos.MapPost("/bulk", async (OnboardleContext _context, List<Photo> photos) =>
            {
                foreach (var photo in photos)
                {
                    await _context.Photos.AddAsync(photo);
                }
                await _context.SaveChangesAsync();
            });
        }
    }
}
