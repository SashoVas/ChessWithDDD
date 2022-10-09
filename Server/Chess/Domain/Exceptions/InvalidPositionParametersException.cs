using Shared.Exceptions;

namespace Domain.Exceptions
{
    public class InvalidPositionParametersException : ChessException
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public InvalidPositionParametersException(int row,int col) : base($"The values for a row and col should be between 0 and 8 but were ({row},{col})")
        {
            Row = row;
            Col = col;
        }
    }
}
