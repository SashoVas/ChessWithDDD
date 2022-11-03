using Application.DTO;
using Application.Exceptions;
using Application.Queries;
using Application.Queries.Handlers;
using Application.Services;
using Moq;
using Xunit;

namespace ChessTests.Application.Queries
{
    public class GetBoardQueryTests
    {
        private readonly Mock<IBoardService> boardServiceMock;

        public GetBoardQueryTests()
        {
            boardServiceMock = new ();
        }

        [Fact]
        public async Task HandleShouldThrowExceptionWhenBoardIsNull()
        {
            var whitePlayerId = Guid.NewGuid();
            var query = new GetBoardQuery(Guid.NewGuid(), whitePlayerId);
            boardServiceMock.Setup(b => b.GetBoard(It.IsAny<Guid>())).Returns(async(Guid id) => null);

            var queryHandler = new GetBoardHandler(boardServiceMock.Object);

            await Assert.ThrowsAsync<TheBoardDoesntExistException>(async()=>await queryHandler.Handle(query,default));
        }
        [Fact]
        public async Task HandleShouldThrowExceptionWhenTheCalerIsNotAPlayer()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var query = new GetBoardQuery(Guid.NewGuid(), Guid.NewGuid());
            boardServiceMock.Setup(b => b.GetBoard(It.IsAny<Guid>())).Returns(async() => new BoardDTO { WhitePlayerId=whitePlayerId, BlackPlayerId= blackPlayerId });
            var queryHandler = new GetBoardHandler(boardServiceMock.Object);

            await Assert.ThrowsAsync<NotAuthorizedToSeeTheBoardException>(async () => await queryHandler.Handle(query, default));
        }
    }
}
