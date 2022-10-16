using Application.DTO;

namespace Application.Services
{
    public interface IBoardService
    {
        Task<BoardDTO> GetBoard(Guid Id);
    }
}
