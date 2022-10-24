using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<ActionResult> Login()
        {

            return Ok();
        }
        [HttpPost("Register")]
        public async Task<ActionResult> Register()
        {
            return Ok();
        }
    }
}
