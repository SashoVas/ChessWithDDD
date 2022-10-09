
using Shared.Exceptions;

namespace Domain.Exceptions
{
    public class InvalidMovesForAPiece : ChessException
    {
        public InvalidMovesForAPiece() : base("Every piece should have moves")
        {
        }
    }
}
