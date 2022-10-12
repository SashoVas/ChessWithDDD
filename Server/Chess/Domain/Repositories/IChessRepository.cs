using Domain.Entities;

namespace Domain.Repositories
{
    public interface IChessRepository
    {
        Task<Board> GetBoard(Guid id);
        Task AddAsync(Board board);
        Task UpdateAsync(Board board);
        Task DeleteAsync(Board board);
    }
}
