using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiContracts;
using WebApiContracts.Mappers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ContactUsController:ControllerBase
    {
        private readonly ContactUsService _contactUsService;
        public ContactUsController(ContactUsService contactUsService)
        {
            _contactUsService = contactUsService;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddSubscription([FromBody] ContactUsContract contactUsContract)
        {
            var result = await _contactUsService.AddFormContactUs(contactUsContract.MapTestToDomain());

            if (result)
            {
                return Ok("Contact us form added successfully.");
            }

            return StatusCode(500, "An error occurred while adding the contact us form.");
        }
    }
}
