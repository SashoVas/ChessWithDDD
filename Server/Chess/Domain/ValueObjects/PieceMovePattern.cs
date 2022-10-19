using Domain.Exceptions;

namespace Domain.ValueObjects
{
    public record PieceMovePattern
    {
        public bool IsRepeatable { get; init; }
        public bool SwapDirections { get; init; }
        public int RowChange { get; }
        public int ColChange { get; }
        private PieceMovePattern()
        {

        }
        public PieceMovePattern(bool isRepeatable, bool swapDirections, int rowChange, int colChange,PieceColor pieceColor)
        {
            if (rowChange >= 8||colChange>=6)
            {
                throw new InvalidValuesForAMoveException(rowChange,colChange);
            }
            if(rowChange == 0 && colChange == 0)
            {
                throw new InvalidValuesForAMoveException(rowChange,colChange);
            }
            IsRepeatable = isRepeatable;
            SwapDirections = swapDirections;
            if (pieceColor==PieceColor.Black)
            {
                rowChange = -rowChange;
                colChange = -colChange;
            }
            RowChange = rowChange;
            ColChange = colChange;
        }
    }
}
