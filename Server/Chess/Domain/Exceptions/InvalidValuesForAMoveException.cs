using Shared.Exceptions;

namespace Domain.Exceptions
{
    public class InvalidValuesForAMoveException : ChessException
    {
        public int RowChange { get; set; }
        public int ColChange { get; set; }
        public InvalidValuesForAMoveException(int rowChange,int colChange) : base("Invalid values for a move")
        {
            RowChange = rowChange;
            ColChange = colChange;
        }
    }
}
