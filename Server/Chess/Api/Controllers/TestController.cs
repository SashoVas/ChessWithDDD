using Application.DTO;
using Application.Services;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        //private readonly IBoardService boardService;
        private readonly IChessRepository repo;

        public TestController(IBoardService boardService, IChessRepository repo)
        {
            //this.boardService = boardService;
            this.repo = repo;
        }

        [HttpGet("Read")]
        public async Task<ActionResult<BoardDTO>> TestReadDb()
        {
            
            //return Ok(await boardService.GetBoard(Guid.NewGuid()));
            return Ok();
        }
        [HttpGet("Write")]
        public async Task<ActionResult> TestWriteDb()
        {
            var board =await repo.GetBoard(Guid.NewGuid());
            return Ok();
        }
        [HttpPost]
        private async Task<ActionResult> CreateBoard()
        {
            
        }
    }
}
