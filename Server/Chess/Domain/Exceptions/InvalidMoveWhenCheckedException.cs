using Shared.Exceptions;

namespace Domain.Exceptions
{
    public class InvalidMoveWhenCheckedException : ChessException
    {
        public InvalidMoveWhenCheckedException() : base("Invalid move because of check")
        {
        }
    }
}
