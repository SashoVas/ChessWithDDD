namespace Domain.ValueObjects
{
    public record PiecePosition
    {
        public int Row { get; }
        public int Col { get; }
        public PiecePosition(int row, int col)
        {
            if (row >= 8 || col >= 8 || row < 0 || col < 0)
            {
                throw new Exception("Invalid position");
            }
            Row = row;
            Col = col;
        }
    }
}
