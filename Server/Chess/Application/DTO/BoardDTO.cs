namespace Application.DTO
{
    public class BoardDTO
    {
        public Guid WhitePlayerId { get; set; }
        public Guid BlackPlayerId { get; set; }
        public string BoardFen { get; set; }
        public bool IsWhiteOnTurn { get; set; }
        public IEnumerable<PieceDTO> Pieces { get; set; }
    }
}
