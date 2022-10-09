using Shared.Exceptions;

namespace Domain.Exceptions
{
    public class WrongTurnException : ChessException
    {
        public WrongTurnException() : base("Its not the player's turn")
        {
        }
    }
}
