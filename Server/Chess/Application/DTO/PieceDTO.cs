namespace Application.DTO
{
    public class PieceDTO
    {
        public string Name { get; set; }
        public string Identifier { get; set; }
        public PiecePositionDTO Position { get; set; }
        public IEnumerable<PieceMovePatternDTO> MovesPatterns { get; set; }
    }
}
