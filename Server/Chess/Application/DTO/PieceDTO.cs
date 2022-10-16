namespace Application.DTO
{
    public class PieceDTO
    {
        public string Name { get; set; }
        public string Identifier { get; set; }
        public PiecePositionDTO Position { get; set; }
        public List<PieceMovePattern> MovesPatterns { get; set; }
    }
}
