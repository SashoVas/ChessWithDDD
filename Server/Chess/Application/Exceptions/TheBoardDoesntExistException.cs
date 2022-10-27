using Shared.Exceptions;

namespace Application.Exceptions
{
    public class TheBoardDoesntExistException : ChessException
    {
        public Guid Id { get; set; }
        public TheBoardDoesntExistException(Guid id) : base($"A board with id:{id} do not exist in the database")
        {
            Id = id;
        }
    }
}
