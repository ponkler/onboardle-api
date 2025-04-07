using Amazon.Runtime.Internal;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Onboardle.Data;
using Onboardle.Endpoints.DriversEndpoints;
using Onboardle.Endpoints.GamesEndpoints;
using Onboardle.Endpoints.PhotosEndpoints;
using Onboardle.Endpoints.TracksEndpoints;

var builder = WebApplication.CreateBuilder(args);

var isProduction = builder.Environment.IsProduction();
string connString;

using (var client = new AmazonSimpleSystemsManagementClient())
{
    var request = new GetParameterRequest
    {
        Name = isProduction ? "/Onboardle/ProdDbConnString" : "/Onboardle/DevDbConnString",
        WithDecryption = true
    };

    var response = await client.GetParameterAsync(request);

    connString = response.Parameter.Value;
}


builder.Services.AddDbContext<OnboardleContext>(options =>
{
        options.UseNpgsql(connString);
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowProdOrigins", policy =>
    {
        policy.WithOrigins("https://onboardle.com", "https://www.onboardle.com")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });

    options.AddPolicy(name: "AllowDevOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});

var app = builder.Build();
app.UseForwardedHeaders();

app.UseCors(isProduction ? "AllowProdOrigins" : "AllowDevOrigins");

app.RegisterGamesEndpoints();
app.RegisterPhotosEndpoints(isProduction);
app.RegisterTracksEndpoints();
app.RegisterDriversEndpoints();

app.Run();