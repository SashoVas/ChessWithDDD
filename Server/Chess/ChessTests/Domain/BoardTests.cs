using Domain.Entities;
using Domain.Events;
using Domain.Factories;
using Domain.ValueObjects;
using Xunit;

namespace ChessTests.Domain
{
    public class BoardTests
    {
        private readonly IBoardFactory boardFactory;

        public BoardTests() 
            => this.boardFactory = new BoardFactory(new PieceFactory());
        private Board GetBoard(string fen,Guid whitePlayerId,Guid blackPlayerId)
        {
            return boardFactory.CreateCustomStandard(fen,whitePlayerId,blackPlayerId);
        }

        [Fact]
        public void MakeAMoveTestShouldMakeAMove()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/8/8/8/8/8/8/8",whitePlayerId,blackPlayerId);
            board.MakeAMove(new PiecePosition(0,0),new PiecePosition(1,1),whitePlayerId);

            var @event=board.Events.First();

            Assert.NotNull(@event);
            Assert.IsType<MakeAMoveBoardDomainEvent>(@event);
            Assert.Null(((MakeAMoveBoardDomainEvent)@event).TakenPiece);
        }
        [Fact]
        public void MakeAMoveTestShouldMakeAMultipleMovesWithoutException()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/1p6/8/8/8/8/8/8", whitePlayerId, blackPlayerId);
            board.MakeAMove(new PiecePosition(0, 0), new PiecePosition(1, 0), whitePlayerId);

            var @event = board.Events.First();

            Assert.NotNull(@event);
            Assert.IsType<MakeAMoveBoardDomainEvent>(@event);
            Assert.Null(((MakeAMoveBoardDomainEvent)@event).TakenPiece);

            board.ClearEvents();

            board.MakeAMove(new PiecePosition(1, 1), new PiecePosition(0, 1), blackPlayerId);

            @event = board.Events.First();

            Assert.NotNull(@event);
            Assert.IsType<MakeAMoveBoardDomainEvent>(@event);
            Assert.Null(((MakeAMoveBoardDomainEvent)@event).TakenPiece);
        }

        [Fact]
        public void MakeAMoveTestShouldMakeAMoveAndTakeAPiece()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/1p6/8/8/8/8/8/8", whitePlayerId, blackPlayerId);
            board.MakeAMove(new PiecePosition(0, 0), new PiecePosition(1, 1), whitePlayerId);

            var @event = board.Events.First();

            Assert.NotNull(@event);
            Assert.IsType<MakeAMoveBoardDomainEvent>(@event);
            Assert.Equal("pawn", ((MakeAMoveBoardDomainEvent)@event).TakenPiece.Name);
        }
    }
}
