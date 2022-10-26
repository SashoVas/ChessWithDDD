using Api.Models.InputModels;
using Application.Commands;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetBoard(Guid BoardId)
        {
            var id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var board =await sender.Send(new GetBoardQuery(BoardId, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))));
            return Ok(board);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateBoard()
        {
            await sender.Send(new CreateStandardBoardCommand(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), Guid.NewGuid()));
            return Ok();
        }
        [Authorize]
        [HttpPut]
        public async Task<ActionResult> MakeAMove(MakeAMoveInputModel input)
        {
            await sender.Send(new MakeAMoveCommand(
                input.BoardId,
                Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                input.StartRow,
                input.StartCol,
                input.EndRow,
                input.EndCol));
            return Ok();
        }
    }
}
