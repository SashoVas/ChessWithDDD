﻿namespace Domain.ValueObjects
{
    public record PieceMovePattern
    {
        public bool IsRepeatable { get; init; }
        public bool SwapDirections { get; init; }
        public int RowChange { get; }
        public int ColChange { get; }
        public PieceMovePattern(bool isRepeatable, bool swapDirections, int rowChange, int colChange,PieceColor pieceColor)
        {
            if (rowChange >= 8||colChange>=6)
            {
                throw new Exception("Invalid move parameters");
            }
            if(rowChange == 0 && colChange == 0)
            {
                throw new Exception("Invalid move parameters");
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
