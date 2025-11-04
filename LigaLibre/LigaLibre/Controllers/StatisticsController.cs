using LigaLibre.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LigaLibre.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StatisticsController(IStatisticsService statisticsService) : ControllerBase
{
    [HttpGet]
    [Route("League")]
    public async Task<IActionResult> GetLeaguesStatistics()
    {
        var statistics = await statisticsService.GetLeaguesStatisticsDtoAsync();
        return Ok(statistics);
    }
    [HttpGet]
    [Route("Matches")]
    public async Task<IActionResult> GetMatchesStatistics()
    {
        var statistics = await statisticsService.GetMatchesStatisticsDtoAsync();
        return Ok(statistics);
    }
    [HttpGet]
    [Route("Players")]
    public async Task<IActionResult> GetPlayersStatistics()
    {
        var statistics = await statisticsService.GetPlayersStatisticsDtoAsync();
        return Ok(statistics);
    }
}
