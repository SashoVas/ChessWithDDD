using Application.DTO;
using Application.Services;
using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    internal class BoardService : IBoardService
    {
        private readonly ReadDbContext dbContext;

        public BoardService(ReadDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<BoardDTO> GetBoard(Guid Id)
        {
            var board =await dbContext.Board.ToListAsync();
            return new BoardDTO();
        }
    }
}
