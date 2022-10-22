using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class ChessRepository : IChessRepository
    {
        private readonly WriteDbContext dbContext;

        public ChessRepository(WriteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(Board board)
        {
            await dbContext.AddAsync(board);
            await dbContext.SaveChangesAsync();
        }

        public Task DeleteAsync(Board board)
        {
            throw new NotImplementedException();
        }

        public async Task<Board> GetBoard(Guid id)
        {
            var board = await dbContext.Boards.FirstOrDefaultAsync(b=>b.Id==id);
            return board;
        }

        public Task UpdateAsync(Board board)
        {
            throw new NotImplementedException();
        }
    }
}
