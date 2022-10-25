using Identity.Models.InputModels;
using Identity.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService identityService;
        private readonly IConfiguration configuration;

        public IdentityController(IIdentityService identityService, IConfiguration configuration)
        {
            this.identityService = identityService;
            this.configuration = configuration;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<object>> Login(LoginInputModel input)
        {
            var secret = configuration.GetSection("AppSettings:Secret").Value;
            try
            {
                var token = await identityService.Login(input.UserName, input.Password, secret);
                return Ok(new { Token = token });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterInputModel input)
        {
            try
            {
                var id = await identityService.Register(input.UserName, input.Password, input.ConfirmPassword);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
