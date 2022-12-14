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

        public async Task DeleteAsync(Board board)
        {
            dbContext.Remove(board);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Board> GetBoard(Guid id)
        {
            var board = await dbContext
                .Boards
                .Include(b=>b.Pieces)
                .ThenInclude(p=>p.Moves)
                .FirstOrDefaultAsync(b=>b.Id==id);
            return board;
        }

        public async Task UpdateAsync(Board board)
        {
            dbContext.Update(board);
            await dbContext.SaveChangesAsync();
        }
    }
}
