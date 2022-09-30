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
        private Board GetBoard(string fen)
        {
            return boardFactory.CreateCustomStandard(fen);
        }

        [Fact]
        public void MakeAMoveTestShouldMakeAMove()
        {
            var board = GetBoard("K7/8/8/8/8/8/8/8");
            board.MakeAMove(new PiecePosition(0,0),new PiecePosition(1,1));

            var @event=board.Events.First();

            Assert.NotNull(@event);
            Assert.IsType<MakeAMoveBoardDomainEvent>(@event);
            Assert.Null(((MakeAMoveBoardDomainEvent)@event).TakenPiece);
        }

        [Fact]
        public void MakeAMoveTestShouldMakeAMoveAndTakeAPiece()
        {
            var board = GetBoard("K7/1p6/8/8/8/8/8/8");
            board.MakeAMove(new PiecePosition(0, 0), new PiecePosition(1, 1));

            var @event = board.Events.First();

            Assert.NotNull(@event);
            Assert.IsType<MakeAMoveBoardDomainEvent>(@event);
            Assert.Equal("pawn", ((MakeAMoveBoardDomainEvent)@event).TakenPiece.Name);
        }
    }
}
