using Application.Commands;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly ISender sender;

        public BoardController(ISender sender)
        {
            this.sender = sender;
        }
        [HttpGet]
        public async Task<ActionResult> GetBoard(Guid BoardId,Guid CallerId)
        {
            var board =await sender.Send(new GetBoardQuery(BoardId,CallerId));
            return Ok(board);
        }
        [HttpPost]
        public async Task<ActionResult> CreateBoard()
        {
            await sender.Send(new CreateStandardBoardCommand(Guid.NewGuid(),Guid.NewGuid()));
            return Ok();
        }
    }
}
