using Shared.Exceptions;

namespace Application.Exceptions
{
    public class NotAuthorizedToSeeTheBoardException : ChessException
    {
        public NotAuthorizedToSeeTheBoardException() : base("Not authorized to see the board")
        {
        }
    }
}
