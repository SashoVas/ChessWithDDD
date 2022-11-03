using Application.Commands;
using Application.Commands.Handlers;
using Application.Exceptions;
using Domain.Events;
using Domain.Factories;
using Domain.Repositories;
using Moq;
using Xunit;

namespace ChessTests.Application.Commands
{
    public class MakeAMoveCommandTests
    {
        private readonly Mock<IChessRepository> repositoryMock;

        public MakeAMoveCommandTests()
        {
            repositoryMock = new Mock<IChessRepository>();
        }

        [Fact]
        public async Task HandleAsyncShouldThrowTheBoardDoesntExistException()
        {
            repositoryMock.Setup(r => r.GetBoard(It.IsAny<Guid>())).Returns(async (Guid id) => null);
            var commandHandler = new MakeAMoveCommandHandler(repositoryMock.Object);
            var command = new MakeAMoveCommand(Guid.NewGuid(), Guid.NewGuid(),1,1,1,1);
            await Assert.ThrowsAsync<TheBoardDoesntExistException>(async()=>await commandHandler.Handle(command,CancellationToken.None));
        }
        [Fact]
        public async Task MakeAMoveShouldMoveAPiece()
        {
            var boardFactory = new BoardFactory(new PieceFactory());
            var whitePlayerId= Guid.NewGuid();
            var blackPlayerId= Guid.NewGuid();
            var board = boardFactory.CreateStandardBoard(whitePlayerId, blackPlayerId);

            repositoryMock.Setup(r => r.GetBoard(It.IsAny<Guid>())).Returns(async (Guid id) => board);

            Assert.Empty(board.Events);
            Assert.Null(board.Pieces.FirstOrDefault(p => p.Position.Row == 2 && p.Position.Col == 1));

            var commandHandler = new MakeAMoveCommandHandler(repositoryMock.Object);
            var command = new MakeAMoveCommand(board.Id, whitePlayerId, 1, 1, 2, 1);
            await commandHandler.Handle(command, CancellationToken.None);

            Assert.NotNull(board.Events.First());
            Assert.IsType<MakeAMoveBoardDomainEvent>(board.Events.First());
            Assert.NotNull(board.Pieces.Where(p=>p.Position.Row==2 && p.Position.Col==1));
        }
    }
}
