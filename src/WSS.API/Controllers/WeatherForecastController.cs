using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WSS.API.Data.Repositories.Account;
using WSS.API.Infrastructure.Config;

namespace WSS.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly IAccountRepo _accountRepo;

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IAccountRepo accountRepo)
    {
        _logger = logger;
        _accountRepo = accountRepo;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get()
    {
        var x = await _accountRepo.GetAccounts().ToListAsync();
        // var x = await _accountRepo.CreateAccount(new Account()
        // {
        //     Username = "asda",
        //     RoleName = RoleName.ADMIN,
        //     Id = Guid.NewGuid(),
        //     RefId = "asd",
        //     Status = 1,
        // });
        return Ok(x);
    }
}