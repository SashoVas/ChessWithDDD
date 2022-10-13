using Domain.Entities;
using Shared.Exceptions;

namespace Domain.Exceptions
{
    public class ForbiddenPiceAddingException : ChessException
    {
        public Piece Piece { get; set; }
        public Guid PlayerId { get; set; }
        public ForbiddenPiceAddingException(Guid playerId,Piece piece) : base("You can only add piece from your color")
        {
            PlayerId = playerId;
            Piece = piece;
        }
    }
}
