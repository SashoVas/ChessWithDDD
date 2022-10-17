namespace Infrastructure.Data.Models
{
    internal class PieceMovePatternReadModel
    {
        public Guid Id { get; set; }
        public bool IsRepeatable { get; set; }
        public bool SwapDirections { get; set; }
        public int RowChange { get; set; }
        public int ColChange { get; set; }
        public Guid PieceReadModelId { get; set; }
        public PieceReadModel Piece { get; set; }
    }
}
