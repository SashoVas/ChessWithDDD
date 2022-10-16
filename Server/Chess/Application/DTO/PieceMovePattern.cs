namespace Application.DTO
{
    public class PieceMovePattern
    {
        public bool IsRepeatable { get; set;  }
        public bool SwapDirections { get; set; }
        public int RowChange { get; set; }
        public int ColChange { get; set; }
    }
}
