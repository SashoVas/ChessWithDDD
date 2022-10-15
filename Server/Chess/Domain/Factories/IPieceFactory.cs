using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Factories
{
    public interface IPieceFactory
    {
        public Piece CreateKing(PiecePosition position,PieceColor color);
        public Piece CreateQueen(PiecePosition position, PieceColor color);
        public Piece CreatePawn(PiecePosition position, PieceColor color);
        public Piece CreateRook(PiecePosition position, PieceColor color);
        public Piece CreateKnight(PiecePosition position, PieceColor color);
        public Piece CreateBishop(PiecePosition position, PieceColor color);
        public Piece CreateCustom(PiecePosition position, PieceColor color,PieceName pieceName,params PieceMovePattern[]pieceMoves);
    }
}
