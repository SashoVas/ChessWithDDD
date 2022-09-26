namespace Domain.ValueObjects
{
    public record PieceMovePattern
    {
        public bool IsRepeatable { get; }
        public bool SwapDirections { get; }
        public int RowChange { get; }
        public int ColChange { get; }
        public PieceMovePattern(bool isRepeatable, bool swapDirections, int rowChange, int colChange)
        {
            if (rowChange >= 8||colChange>=6)
            {
                throw new Exception("Invalid move parameters");
            }
            if(rowChange<0 || colChange < 0)
            {
                throw new Exception("Invalid move parameters");
            }
            if(rowChange == 0 && colChange == 0)
            {
                throw new Exception("Invalid move parameters");
            }
            IsRepeatable = isRepeatable;
            SwapDirections = swapDirections;
            RowChange = rowChange;
            ColChange = colChange;
        }
    }
}
