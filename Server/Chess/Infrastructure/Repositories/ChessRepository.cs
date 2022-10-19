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

        public Task AddAsync(Board board)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Board board)
        {
            throw new NotImplementedException();
        }

        public async Task<Board> GetBoard(Guid id)
        {
            var board = await dbContext.Board.FirstOrDefaultAsync();
            return board;
        }

        public Task UpdateAsync(Board board)
        {
            throw new NotImplementedException();
        }
    }
}
