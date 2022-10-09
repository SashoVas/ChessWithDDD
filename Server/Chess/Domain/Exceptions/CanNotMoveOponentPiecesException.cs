
using Shared.Exceptions;

namespace Domain.Exceptions
{
    public class CanNotMoveOponentPiecesException : ChessException
    {
        public CanNotMoveOponentPiecesException() : base("You can't move oponent pieces")
        {
        }
    }
}
