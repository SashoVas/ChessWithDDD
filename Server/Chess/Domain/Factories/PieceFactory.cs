using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Factories
{
    public class PieceFactory : IPieceFactory
    {
        public Piece CreateCustom(PiecePosition position, PieceColor color)
        {
            throw new NotImplementedException();
        }

        Piece IPieceFactory.CreateBishop(PiecePosition position, PieceColor color)
        {
            var bishopMoves = new PieceMovePattern(true, true, 1, 1, color);
            return new Piece(Guid.NewGuid(), new PieceName("bishop", color), position,color, bishopMoves);
        }

        Piece IPieceFactory.CreateKing(PiecePosition position, PieceColor color)
        {
            var verticalMoves = new PieceMovePattern(false, true, 1, 0,color);
            var horizontalMoves = new PieceMovePattern(false, true, 0, 1, color);
            var bishopMoves = new PieceMovePattern(false, true, 1, 1, color);
            return new(Guid.NewGuid(), new PieceName("king", color), position, color, verticalMoves , horizontalMoves, bishopMoves);
        }

        Piece IPieceFactory.CreateKnight(PiecePosition position, PieceColor color)
        {
            var knightMoves1 = new PieceMovePattern(true, true, 1, 2, color);
            var knightMoves2 = new PieceMovePattern(true, true, 2, 1, color);
            return new Piece(Guid.NewGuid(),new PieceName("knight",color,"n") , position, color, knightMoves1, knightMoves2);
        }

        Piece IPieceFactory.CreatePawn(PiecePosition position, PieceColor color)
        {
            var verticalMoves = new PieceMovePattern(false, false, 1, 0, color);
            return new Piece(Guid.NewGuid(), new PieceName("pawn", color), position, color, verticalMoves);
        }

        Piece IPieceFactory.CreateQueen(PiecePosition position, PieceColor color)
        {
            var verticalMoves = new PieceMovePattern(true, true, 1, 0, color);
            var horizontalMoves = new PieceMovePattern(true, true, 0, 1, color);
            var bishopMoves = new PieceMovePattern(true, true, 1, 1, color);
            return new(Guid.NewGuid(), new PieceName("queen", color), position, color, verticalMoves, horizontalMoves, bishopMoves);
        }

        Piece IPieceFactory.CreateRook(PiecePosition position, PieceColor color)
        {
            var verticalMoves = new PieceMovePattern(true, true, 1, 0, color);
            var horizontalMoves = new PieceMovePattern(true, true, 0, 1, color);
            return new Piece(Guid.NewGuid(), new PieceName("rook", color), position, color, verticalMoves, horizontalMoves);
        }
    }
}
