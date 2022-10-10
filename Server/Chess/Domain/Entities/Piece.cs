using Domain.Exceptions;
using Domain.ValueObjects;
using Shared.Domain;

namespace Domain.Entities
{
    public sealed class Piece : Entity
    {
        public PieceName Name { get;private set; }
        public PiecePosition Position { get;private set; }
        public PieceColor Color { get; private init; }
        public bool IsTaken { get; private set; }
        public List<PieceMovePattern> Moves { get; private init; }
        public Piece(Guid Id, PieceName name, PiecePosition position,PieceColor color, params PieceMovePattern[] moves) : base(Id)
        {
            if (moves.Length==0)
            {
                throw new InvalidMovesForAPiece() ;
            }
            Name = name;
            Position = position;
            Color = color;
            Moves = moves.ToList();
            IsTaken = false;
        }
        internal void MakeAMove(PiecePosition move) => Position = move;
        internal void TakePiece() => IsTaken = true;
    }
}
