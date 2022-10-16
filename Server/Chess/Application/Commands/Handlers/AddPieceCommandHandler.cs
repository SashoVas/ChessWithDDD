using Application.Abstractions.Commands;
using Domain.Factories;
using Domain.Repositories;
using Domain.ValueObjects;
using MediatR;

namespace Application.Commands.Handlers
{
    internal class AddPieceCommandHandler : ICommandHandler<AddPieceCommand>
    {
        private readonly IChessRepository chessRepository;
        private readonly IPieceFactory pieceFactory;

        public AddPieceCommandHandler(IChessRepository chessRepository, IPieceFactory pieceFactory)
        {
            this.chessRepository = chessRepository;
            this.pieceFactory = pieceFactory;
        }

        public async Task<Unit> Handle(AddPieceCommand request, CancellationToken cancellationToken)
        {
            var board =await chessRepository.GetBoard(request.BoardId);

            if (board is null )
            {
                throw new NullReferenceException();
            }

            var color = request.IsWhiteColor ? PieceColor.White : PieceColor.Black;
            var picePosition = new PiecePosition(request.Position.Row, request.Position.Col);
            var name = new PieceName(request.Name, color);
            var moves = request.Moves.Select(x=>new PieceMovePattern(x.IsRepeatable,x.SwapDirections,x.RowChange,x.ColChange,color)).ToArray();
            
            var piece = pieceFactory.CreateCustom(
                picePosition,
                color,
                name,
                moves);

            board.AddPiece(piece,request.UserId);

            return Unit.Value;
        }
    }
}