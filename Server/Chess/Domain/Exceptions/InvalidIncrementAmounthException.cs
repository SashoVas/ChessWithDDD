using Shared.Domain;
using Shared.Exceptions;

namespace Domain.Exceptions
{
    public class InvalidIncrementAmounthException : ChessException
    {
        public InvalidIncrementAmounthException() : base($"Invalid amount for increment value. It should be between:" +
            $"{DomainConstants.MinIncrementAmounthInSeconds} and {DomainConstants.MaxIncrementAmounthPercentageFromGameDuration}% of the game duration.")
        {
        }
    }
}
