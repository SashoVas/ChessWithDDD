using Application.Abstractions.Queries;
using Application.DTO;
using Application.Exceptions;
using Application.Services;

namespace Application.Queries.Handlers
{
    public class GetBoardHandler : IQueryHandler<GetBoardQuery, BoardDTO>
    {
        private readonly IBoardService boardService;

        public GetBoardHandler(IBoardService boardService) 
            => this.boardService = boardService;

        public async Task<BoardDTO> Handle(GetBoardQuery request, CancellationToken cancellationToken)
        {
            var boardDto =await boardService.GetBoard(request.BoardId);

            if (boardDto is null)
            {
                throw new TheBoardDoesntExistException(request.BoardId);
            }

            if (boardDto.WhitePlayerId != request.CallerId && boardDto.BlackPlayerId != request.CallerId)
            {
                throw new NotAuthorizedToSeeTheBoardException();
            }

            return boardDto;
        }
    }
}
