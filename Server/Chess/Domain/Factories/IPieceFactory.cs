using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Factories
{
    public interface IPieceFactory
    {
        internal Piece CreateKing(PiecePosition position,PieceColor color);
        internal Piece CreateQueen(PiecePosition position, PieceColor color);
        internal Piece CreatePawn(PiecePosition position, PieceColor color);
        internal Piece CreateRook(PiecePosition position, PieceColor color);
        internal Piece CreateKnight(PiecePosition position, PieceColor color);
        internal Piece CreateBishop(PiecePosition position, PieceColor color);
        public Piece CreateCustom(PiecePosition position, PieceColor color);
    }
}
