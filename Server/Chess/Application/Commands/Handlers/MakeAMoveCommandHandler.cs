using Application.Abstractions.Commands;
using Application.Exceptions;
using Domain.Repositories;
using Domain.ValueObjects;
using MediatR;

namespace Application.Commands.Handlers
{
    public sealed class MakeAMoveCommandHandler : ICommandHandler<MakeAMoveCommand>
    {
        private readonly IChessRepository repo;

        public MakeAMoveCommandHandler(IChessRepository repo)
        {
            this.repo = repo;
        }

        public async Task<Unit> Handle(MakeAMoveCommand request, CancellationToken cancellationToken)
        {
            var board=await repo.GetBoard(request.BoardId);
            if (board is null)
            {
                throw new TheBoardDoesntExistException(request.BoardId);
            }
            var startPosition = new PiecePosition(request.StartRow,request.StartCol);
            var move = new PiecePosition(request.EndRow,request.EndCol);
            board.MakeAMove(startPosition,move,request.PlayerId);
            return Unit.Value;
        }
    }
}
