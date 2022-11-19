using Shared.Domain;
using Shared.Exceptions;

namespace Domain.Exceptions
{
    public class InvalidGameDurationException : ChessException
    {
        TimeSpan Duration { get; set; }
        public InvalidGameDurationException(TimeSpan duration) : base($"The game duration should be between:" +
            $"{DomainConstants.MinTurnDurationInMinutes} and {DomainConstants.MaxTurnDurationInMinutes}, but it was {duration}")
        {
            Duration = duration;
        }
    }
}
