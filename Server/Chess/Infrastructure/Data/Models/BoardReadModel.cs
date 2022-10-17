namespace Infrastructure.Data.Models
{
    internal class BoardReadModel
    {
        public BoardReadModel()
        {
            Pieces = new HashSet<PieceReadModel>();
        }
        public Guid Id { get; set; }
        public ICollection<PieceReadModel> Pieces { get; set; }
        public string Fen { get;  set; }
        public Guid WhitePlayerId { get; set; }
        public Guid BlackPlayerId { get; set; }
        public bool IsWhiteOnTurn { get; set; }
    }
}
