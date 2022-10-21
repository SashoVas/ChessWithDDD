using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestReadController : ControllerBase
    {
        private readonly ISender sender;

        public TestReadController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        public async Task<ActionResult> TestRead(Guid BoardId,Guid Caller)
        {

            var board =await sender.Send(new GetBoardQuery(BoardId, Caller));
            return Ok(board);
        }
    }
}
