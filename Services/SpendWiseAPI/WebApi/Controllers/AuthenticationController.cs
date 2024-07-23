using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Services;
using WebApiContracts;
using WebApiContracts.Mappers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthorizationService _authorizationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(AuthorizationService authorizationService, ILogger<AuthenticationController> logger)
        {
            _authorizationService = authorizationService;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> RegisterUser([FromBody] UserCredentialsContract credentialsContract)
        {
            var result = await this._authorizationService.RegisterUser(credentialsContract.MapTestToDomain());
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<string>> LoginUser([FromBody] UserCredentialsContract1 credentialsContract)
        {
            var result = await this._authorizationService.LoginUser(credentialsContract.MapTestToDomain());

            _logger.LogInformation("Logged in as user: "+result.Name);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = IdentityData.AdminUserPolicyName)]
        public async Task<ActionResult<string>> GiveUserAdminRights([FromBody] string email)
        {
            var result = await this._authorizationService.GiveUserAdminRights(email);

            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<string>> SaveAccountSettings([FromBody] UserAccount userAccount)
        {
            var result = await this._authorizationService.SaveAccountSettings(userAccount.MapTestToDomain());
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<string>> ResetPassword([FromBody] PasswordReset userAccount)
        {
            var result = await this._authorizationService.ResetPassword(userAccount.MapTestToDomain());
            return Ok(result);
        }

    }
}
