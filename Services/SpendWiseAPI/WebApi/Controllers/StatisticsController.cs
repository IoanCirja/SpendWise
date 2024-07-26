using Application.Services;
using Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StatisticsController:ControllerBase
    {
        private readonly StatisticsService _statisticsService;
        public StatisticsController(StatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }
        [HttpGet ("{user_id}")]
        public async Task<IActionResult> GetStatistics(Guid user_id)
        {
            var result = _statisticsService.GetStatistics(user_id);
            return Ok(result);
        }
    }
}
