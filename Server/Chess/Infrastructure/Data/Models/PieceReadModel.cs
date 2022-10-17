namespace Infrastructure.Data.Models
{
    internal class PieceReadModel
    {
        public PieceReadModel()
        {
            Moves = new HashSet<PieceMovePatternReadModel>();
        }
        public Guid Id { get; set; }
        public string Name { get;  set; }
        public string Identifier { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public PieceColorReadModel Color { get;  set; }
        public bool IsTaken { get;  set; }
        public ICollection<PieceMovePatternReadModel> Moves { get;  set; }
        public BoardReadModel Board { get; set; }
        public Guid BoardId { get; set; }
    }
}
