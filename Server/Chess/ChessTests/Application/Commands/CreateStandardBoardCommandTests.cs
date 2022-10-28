using Application.Commands;
using Application.Commands.Handlers;
using Domain.Entities;
using Domain.Factories;
using Domain.Repositories;
using Moq;
using Xunit;

namespace ChessTests.Application.Commands
{
    //CreateStandardBoardCommandHandler
    public class CreateStandardBoardCommandTests
    {
        [Fact]
        public async Task HandleAsyncShouldCreateABoard()
        {
            var repo = new Mock<IChessRepository>();
            var result = new List<Board>();
            repo.Setup(r => r.AddAsync(It.IsAny<Board>())).Returns(async(Board board)=>result.Add(board));
            var pieceFactory = new PieceFactory();
            var boardFactory = new BoardFactory(pieceFactory);
            var commandHandler = new CreateStandardBoardCommandHandler(repo.Object, boardFactory);
            var command = new CreateStandardBoardCommand(Guid.NewGuid(),Guid.NewGuid());
            await commandHandler.Handle(command, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal(32, result[0].Pieces.Count);
        }
    }
}
