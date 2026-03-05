using MongoDB.Driver;
using TodoApi.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization; // Add this line

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// --- Localization Configuration ---
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("zh-Hant-TW") };
    options.DefaultRequestCulture = new RequestCulture("zh-Hant-TW");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});
// --- End Localization Configuration ---

// --- MongoDB Configuration ---
// Your MongoDB Atlas Connection String
// Use the password for 'speedken' user: Admin818!
const string connectionUri = "mongodb+srv://speedken:Admin818@cluster0.9e6tav7.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

// Register IMongoClient as a singleton
builder.Services.AddSingleton<IMongoClient>(new MongoClient(connectionUri));

// Register TodoService as a singleton
builder.Services.AddSingleton<TodoService>();
// --- End MongoDB Configuration ---

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRequestLocalization(); // Add this line to enable localization middleware

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers(); // Add this line to map controller routes

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
