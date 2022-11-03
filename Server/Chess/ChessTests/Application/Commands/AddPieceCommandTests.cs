using Application.Commands;
using Application.Commands.Handlers;
using Application.Exceptions;
using Domain.Factories;
using Domain.Repositories;
using Moq;
using Xunit;

namespace ChessTests.Application.Commands
{
    public class AddPieceCommandTests
    {
        private readonly Mock<IChessRepository> chessRepositoryMock;

        public AddPieceCommandTests()
        {
            chessRepositoryMock = new Mock<IChessRepository>();
        }

        [Fact]
        public async Task HandleAsyncShouldThrowTheBoardDoesntExistException()
        {
            chessRepositoryMock.Setup(r => r.GetBoard(It.IsAny<Guid>())).Returns(async (Guid id) => null);
            var pieceFactory = new PieceFactory();
            var commandHandler = new AddPieceCommandHandler(chessRepositoryMock.Object, pieceFactory);
            var command = new AddPieceCommand(Guid.NewGuid(), Guid.NewGuid(), "randomName", null, true, null);
            await Assert.ThrowsAsync<TheBoardDoesntExistException>(async () => await commandHandler.Handle(command, CancellationToken.None));
        }
    }
}
