using Microsoft.AspNetCore.Mvc;

namespace UnreliableApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    private static int RequestCount = 0;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet("reliable")]
    public IEnumerable<WeatherForecast> GetReliable()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("unreliable")]
    public IEnumerable<WeatherForecast> GetUnreliable()
    {
        RequestCount++;

        // Only succeed every 3 attempts
        if (RequestCount % 3 == 0)
        {

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        else
        {
            throw new Exception("I'm Unreliable :p");
        }
    }

    [HttpGet("borked")]
    public IActionResult GetBorked()
    {
        throw new NotImplementedException("I'm VERY Unreliable");
    }

    [HttpGet("Slow")]
    public async Task<IEnumerable<WeatherForecast>> GetSlowAsync()
    {
        RequestCount++;

        // Be very slow every other attempt
        if (RequestCount % 2 == 0)
        {
            await Task.Delay(15000);
        }

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();
    }
}
