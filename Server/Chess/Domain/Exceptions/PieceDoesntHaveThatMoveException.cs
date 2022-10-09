using Shared.Exceptions;

namespace Domain.Exceptions
{
    public class PieceDoesntHaveThatMoveException : ChessException
    {
        public PieceDoesntHaveThatMoveException() : base("The piece cannot make that move")
        {
        }
    }
}
