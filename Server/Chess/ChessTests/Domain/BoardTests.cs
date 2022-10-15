using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;
using Domain.Factories;
using Domain.ValueObjects;
using Xunit;

namespace ChessTests.Domain
{
    public class BoardTests
    {
        private readonly IBoardFactory boardFactory;
        private readonly IPieceFactory pieceFactory;

        public BoardTests()
        {
            pieceFactory = new PieceFactory();
            boardFactory = new BoardFactory(pieceFactory);
        }

        private Board GetBoard(string fen,Guid whitePlayerId,Guid blackPlayerId)
        {
            return boardFactory.CreateCustomStandard(fen,whitePlayerId,blackPlayerId);
        }

        [Fact]
        public void MakeAMoveTestShouldMakeAMove()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/8/8/8/8/8/8/k7", whitePlayerId,blackPlayerId);
            board.MakeAMove(new PiecePosition(0,0),new PiecePosition(1,1),whitePlayerId);

            var @event=board.Events.First();

            Assert.NotNull(@event);
            Assert.IsType<MakeAMoveBoardDomainEvent>(@event);
            Assert.Null(((MakeAMoveBoardDomainEvent)@event).TakenPiece);
        }
        [Fact]
        public void MakeAMoveTestShouldThrowExceptionWhenOnTheDestinationIsAPieceFromSameColor()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/1P6/8/8/8/8/8/k7", whitePlayerId, blackPlayerId);
            Assert.Throws<TooManyPiecesException>(() =>board.MakeAMove(new PiecePosition(0, 0), new PiecePosition(1, 1), whitePlayerId));
        }
        [Fact]
        public void MakeAMoveTestShouldThrowExceptionWhenThereIsNoPieceToMove()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/1P6/8/8/8/8/8/k7", whitePlayerId, blackPlayerId);
            Assert.Throws<PositionShouldBeAPieceException>(() => board.MakeAMove(new PiecePosition(1, 0), new PiecePosition(1, 1), whitePlayerId));
        }
        [Fact]
        public void MakeAMoveTestShouldThrowExceptionWhenThePieceCantMakeTheMove()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/1P6/8/8/8/8/8/k7", whitePlayerId, blackPlayerId);
            Assert.Throws<PieceDoesntHaveThatMoveException>(() => board.MakeAMove(new PiecePosition(0, 0), new PiecePosition(6, 6), whitePlayerId));
        }
        [Fact]
        public void MakeAMoveTestShouldThrowExceptionWhenItsNotThatColorsTurn()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/8/8/8/8/8/8/k7", whitePlayerId, blackPlayerId);
            Assert.Throws<WrongTurnException>(() => board.MakeAMove(new PiecePosition(0, 0), new PiecePosition(1,1), blackPlayerId));
        }
        [Fact]
        public void MakeAMoveTestShouldThrowExceptionWhenTryingToMoveOponentPiece()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/8/8/8/8/8/8/k7", whitePlayerId, blackPlayerId);
            Assert.Throws<CanNotMoveOponentPiecesException>(() => board.MakeAMove(new PiecePosition(7, 0), new PiecePosition(6, 0), whitePlayerId));
        }
        [Fact]
        public void MakeAMoveTestShouldMakeAMultipleMovesWithoutException()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/1p6/8/8/8/8/8/k7", whitePlayerId, blackPlayerId);
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
            var board = GetBoard("K7/1p6/8/8/8/8/8/k7", whitePlayerId, blackPlayerId);
            board.MakeAMove(new PiecePosition(0, 0), new PiecePosition(1, 1), whitePlayerId);

            var @event = board.Events.First();

            Assert.NotNull(@event);
            Assert.IsType<MakeAMoveBoardDomainEvent>(@event);
            Assert.Equal("pawn", ((MakeAMoveBoardDomainEvent)@event).TakenPiece.Name);
        }
        [Fact]
        public void AddPieceShouldAddPieceToTheBoard()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/1p6/8/8/8/8/8/k7", whitePlayerId, blackPlayerId);
            var piece = pieceFactory.CreateKnight(new PiecePosition(5, 5), PieceColor.White);
            board.AddPiece(piece,whitePlayerId);

            var @event = board.Events.First();
            Assert.NotNull(@event);
            Assert.IsType<AddPieceToBoardDomainEvent>(@event);
            Assert.Equal(piece,((AddPieceToBoardDomainEvent)@event).Piece);
        }
        [Fact]
        public void AddCustomPieceShouldAddPieceToTheBoard()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/1p6/8/8/8/8/8/k7", whitePlayerId, blackPlayerId);
            var piece = pieceFactory.CreateCustom(
                new PiecePosition(5, 5),
                PieceColor.White,new PieceName("test",PieceColor.White),
                new PieceMovePattern(true,true,0,1,PieceColor.White));
            board.AddPiece(piece, whitePlayerId);

            var @event = board.Events.First();
            Assert.NotNull(@event);
            Assert.IsType<AddPieceToBoardDomainEvent>(@event);
            Assert.Equal(piece, ((AddPieceToBoardDomainEvent)@event).Piece);
        }
        [Fact]
        public void AddPieceShouldThrowAnExeptionWhenThereIsPieceOnTheSquare()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/1p6/8/8/8/8/8/k7", whitePlayerId, blackPlayerId);
            var piece = pieceFactory.CreateKnight(new PiecePosition(1, 1), PieceColor.White);
            
            Assert.Throws<TooManyPiecesException>(()=>board.AddPiece(piece,whitePlayerId));
        }
        [Fact]
        public void AddPiecesShouldAddMultiplePiecesToTheBoard()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/1p6/8/8/8/8/8/k7", whitePlayerId, blackPlayerId);
            var piece1 = pieceFactory.CreateKnight(new PiecePosition(2, 2), PieceColor.White);
            var piece2 = pieceFactory.CreateKnight(new PiecePosition(3, 3), PieceColor.White);
            var piece3 = pieceFactory.CreateKnight(new PiecePosition(4, 4), PieceColor.White);
            var piece4 = pieceFactory.CreateKnight(new PiecePosition(5, 5), PieceColor.White);
            var list = new List<Piece> { piece1, piece2, piece3, piece4 };
            board.AddPieces(list,whitePlayerId);

            Assert.Equal(4,board.Events.Count());
            int el = 0;
            foreach (var e in board.Events)
            {
                Assert.IsType<AddPieceToBoardDomainEvent>(e);
                Assert.Equal(list[el], ((AddPieceToBoardDomainEvent)e).Piece);
                el++;
            }
        }
        [Fact]
        public void AddPieceShouldThrowExceptionWhenAddingPieceThatIsNotYourColor()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/1p6/8/8/8/8/8/k7", whitePlayerId, blackPlayerId);
            var piece = pieceFactory.CreateKnight(new PiecePosition(5, 5), PieceColor.White);

            Assert.Throws<ForbiddenPiceAddingException>(()=>board.AddPiece(piece, blackPlayerId));            
        }
        [Fact]
        public void AddPiecesShouldThrowExceptionWhenAddingPieceThatIsNotYourColor()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/1p6/8/8/8/8/8/k7", whitePlayerId, blackPlayerId);
            var piece1 = pieceFactory.CreateKnight(new PiecePosition(2, 2), PieceColor.White);
            var piece2 = pieceFactory.CreateKnight(new PiecePosition(3, 3), PieceColor.White);
            var piece3 = pieceFactory.CreateKnight(new PiecePosition(4, 4), PieceColor.White);
            var piece4 = pieceFactory.CreateKnight(new PiecePosition(5, 5), PieceColor.Black);
            var list = new List<Piece> { piece1, piece2, piece3, piece4 };
            
            Assert.Throws < ForbiddenPiceAddingException >(()=>board.AddPieces(list, whitePlayerId));
        }
        [Fact]
        public void MakeAMoveShouldThrowCheckMateException()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("kb6/p7/8/1B6/8/8/8/K7", whitePlayerId, blackPlayerId);

            Assert.Throws<NotImplementedException>(()=>board.MakeAMove(new PiecePosition(3, 1), new PiecePosition(2, 2), whitePlayerId));
        }
        [Fact]
        public void MakeAMoveShouldThrowWhenTheKingIsUnderCheck()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/P7/2b5/8/8/8/8/k7", whitePlayerId, blackPlayerId);

            Assert.Throws<InvalidMoveWhenCheckedException>(() => board.MakeAMove(new PiecePosition(1, 0), new PiecePosition(2, 0), whitePlayerId));
        }
        [Fact]
        public void MakeAMoveShouldMakeAMoveWhenEscapeingCheck()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("K7/P7/2b5/8/8/8/8/k7", whitePlayerId, blackPlayerId);

            board.MakeAMove(new PiecePosition(0, 0), new PiecePosition(0, 1), whitePlayerId);

            var @event = board.Events.First();

            Assert.NotNull(@event);
            Assert.IsType<MakeAMoveBoardDomainEvent>(@event);
            Assert.Null(((MakeAMoveBoardDomainEvent)@event).TakenPiece);
        }
        [Fact]
        public void MakeAMoveShouldPutThePlayerInCheckButNotInMateWithAvelableRookMove()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("kr6/p7/8/1B6/8/8/8/K7", whitePlayerId, blackPlayerId);
            board.MakeAMove(new PiecePosition(3, 1), new PiecePosition(2, 2), whitePlayerId);

            var @event = board.Events.First();

            Assert.NotNull(@event);
            Assert.IsType<MakeAMoveBoardDomainEvent>(@event);
            Assert.Equal("bishop", ((MakeAMoveBoardDomainEvent)@event).MovedPiece.Name.Name);
            Assert.Null(((MakeAMoveBoardDomainEvent)@event).TakenPiece);

        }
        [Fact]
        public void MakeAMoveShouldPutThePlayerInCheckButNotInMateWithAvelableKingMove()
        {
            var whitePlayerId = Guid.NewGuid();
            var blackPlayerId = Guid.NewGuid();
            var board = GetBoard("k7/p7/8/1B6/8/8/8/K7", whitePlayerId, blackPlayerId);
            board.MakeAMove(new PiecePosition(3, 1), new PiecePosition(2, 2), whitePlayerId);

            var @event = board.Events.First();

            Assert.NotNull(@event);
            Assert.IsType<MakeAMoveBoardDomainEvent>(@event);
            Assert.Equal("bishop", ((MakeAMoveBoardDomainEvent)@event).MovedPiece.Name.Name);
            Assert.Null(((MakeAMoveBoardDomainEvent)@event).TakenPiece);
        }
    }
}
