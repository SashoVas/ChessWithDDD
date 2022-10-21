using Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestWriteController : ControllerBase
    {
        private readonly ISender sender;

        public TestWriteController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet()]
        public async Task<ActionResult> TestWriteDb()
        {
            await sender.Send(new CreateStandardBoardCommand(Guid.NewGuid(),Guid.NewGuid()));
            return Ok();
        }
    }
}
