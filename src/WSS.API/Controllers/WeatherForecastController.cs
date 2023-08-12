using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WSS.API.Data.Repositories.Account;

namespace WSS.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IAccountRepo _accountRepo;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IAccountRepo accountRepo)
    {
        _logger = logger;
        _accountRepo = accountRepo;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get()
    {
        var x = await  this._accountRepo.GetAccounts().ToListAsync();

        return Ok(x);
    }
}