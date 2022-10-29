using Shared.Domain;
using Shared.Exceptions;

namespace Domain.Exceptions
{
    public class InvalidPieceNameException : ChessException
    {
        public string Name { get; set; }
        public InvalidPieceNameException(string name) 
            : base($"Invalid name for a piece.The name should be between " +
                  $"{DomainConstants.MinNameLength} and " +
                  $"{DomainConstants.MaxNameLength}")
        {
            Name = name;
        }
    }
}
