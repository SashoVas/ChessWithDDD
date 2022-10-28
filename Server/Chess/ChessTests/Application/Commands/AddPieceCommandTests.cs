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
        [Fact]
        public async Task HandleAsyncShouldThrowTheBoardDoesntExistException()
        {
            var repo = new Mock<IChessRepository>();
            repo.Setup(r => r.GetBoard(It.IsAny<Guid>())).Returns(async (Guid id) => null);
            var pieceFactory = new PieceFactory();
            var commandHandler = new AddPieceCommandHandler(repo.Object,pieceFactory);
            var command = new AddPieceCommand(Guid.NewGuid(),Guid.NewGuid(),"randomName",null,true,null);
            await Assert.ThrowsAsync<TheBoardDoesntExistException>(async () => await commandHandler.Handle(command, CancellationToken.None));
        }
    }
}
