using Domain.ValueObjects;
using Shared.Exceptions;

namespace Domain.Exceptions
{
    public class TooManyPiecesException : ChessException
    {
        public PiecePosition Position { get; set; }
        public TooManyPiecesException(PiecePosition position) : base(position!=null?$"Too many pieces on ({position.Row},{position.Col})": "Too many pieces on the board")
        {
            Position = position;
        }
    }
}
