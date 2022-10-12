using Shared.Exceptions;

namespace Domain.Exceptions
{
    public class InvalidPlayerIdException : ChessException
    {
        public Guid Value { get; set; }
        public InvalidPlayerIdException(Guid value) : base("Inproper id value")
        {
            Value = value;
        }
    }
}
