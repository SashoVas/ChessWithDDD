using Application.Abstractions.Queries;
using Application.DTO;
using Application.Services;

namespace Application.Queries.Handlers
{
    internal class GetBoardHandler : IQueryHandler<GetBoardQuery, BoardDTO>
    {
        private readonly IBoardService boardService;

        public GetBoardHandler(IBoardService boardService) 
            => this.boardService = boardService;

        public async Task<BoardDTO> Handle(GetBoardQuery request, CancellationToken cancellationToken)
        {
            var boardDto =await boardService.GetBoard(request.BoardId);

            if (boardDto.WhitePlayerId != request.CallerId && boardDto.BlackPlayerId != request.CallerId)
            {
                throw new InvalidOperationException();
            }

            return boardDto;
        }
    }
}
