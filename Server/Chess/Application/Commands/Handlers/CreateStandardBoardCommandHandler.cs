using Application.Abstractions.Commands;
using Domain.Factories;
using Domain.Repositories;
using MediatR;

namespace Application.Commands.Handlers
{
    public sealed class CreateStandardBoardCommandHandler: ICommandHandler<CreateStandardBoardCommand>
    {
        private readonly IChessRepository repo;
        private readonly IBoardFactory factory;

        public CreateStandardBoardCommandHandler(IChessRepository repo, IBoardFactory factory)
        {
            this.repo = repo;
            this.factory = factory;
        }

        public async Task<Unit> Handle(CreateStandardBoardCommand request, CancellationToken cancellationToken)
        {
            var board = factory.CreateStandardBoard(request.WhitePlayerId,request.BlackPlayerId);
            await repo.AddAsync(board);

            return Unit.Value;
        }
    }
}
