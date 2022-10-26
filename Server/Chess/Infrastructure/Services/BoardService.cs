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

        public async Task<BoardDTO> GetBoard(Guid id)
        {
            var board = await dbContext.Boards
                .Include(b=>b.Pieces)
                .ThenInclude(p=>p.Moves)
                .AsNoTracking()
                .FirstOrDefaultAsync(b=>b.Id==id);
            return new BoardDTO {
                BlackPlayerId=board.BlackPlayerId,
                WhitePlayerId=board.WhitePlayerId,
                BoardFen=board.Fen,
                IsWhiteOnTurn=board.IsWhiteOnTurn,
                Pieces=board.Pieces.Select(p=>new PieceDTO
                {
                    Identifier=p.Identifier,
                    Name=p.Name,
                    Position=new PiecePositionDTO
                    {
                        Row=p.Row,
                        Col=p.Col
                    },
                    MovesPatterns=p.Moves.Select(m=>new PieceMovePatternDTO
                    {
                        SwapDirections=m.SwapDirections,
                        ColChange=m.ColChange,
                        IsRepeatable=m.IsRepeatable,
                        RowChange=m.RowChange
                    })
                })
            };
        }
    }
}
