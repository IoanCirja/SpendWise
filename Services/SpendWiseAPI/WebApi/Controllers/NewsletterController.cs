using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class NewsletterController:ControllerBase
    {
        private readonly NewsLetterService _newsletterService;
        public NewsletterController(NewsLetterService newsletterService)
        {
            _newsletterService = newsletterService;
        }
        [HttpPost]
        public async Task<IActionResult> AddSubscription(string email)
        {
            var result = await _newsletterService.AddSubscription(email);

            if (result)
            {
                return Ok("Subscription added successfully.");
            }

            return StatusCode(500, "An error occurred while adding the subscription.");
        }
    }
}
