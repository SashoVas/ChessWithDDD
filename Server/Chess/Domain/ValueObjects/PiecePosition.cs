using Domain.Exceptions;

namespace Domain.ValueObjects
{
    public record PiecePosition
    {
        public int Row { get; init; }
        public int Col { get; init; }
        private PiecePosition()
        {

        }
        public PiecePosition(int row, int col)
        {
            if (row >= 8 || col >= 8 || row < 0 || col < 0)
            {
                throw new InvalidPositionParametersException(row,col);
            }
            Row = row;
            Col = col;
        }
    }
}
