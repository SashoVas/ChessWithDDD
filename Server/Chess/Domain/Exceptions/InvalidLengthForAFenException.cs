using Shared.Exceptions;

namespace Domain.Exceptions
{
    public class InvalidLengthForAFenException : ChessException
    {
        public InvalidLengthForAFenException() : base("Fen length should be exactly 8")
        {
        }
    }
}
