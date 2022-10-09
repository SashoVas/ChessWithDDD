
using Domain.ValueObjects;
using Shared.Exceptions;

namespace Domain.Exceptions
{
    internal class PositionShouldBeAPieceException : ChessException
    {
        public PiecePosition Position { get; set; }
        public PositionShouldBeAPieceException(PiecePosition position) : base($"The position ({position.Row},{position.Col}) should be a piece")
        {
            Position = position;
        }
    }
}
