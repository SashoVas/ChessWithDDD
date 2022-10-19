using Domain.Entities;
using Domain.Exceptions;

namespace Domain.ValueObjects
{
    public record FenIdentifier
    {
        public string[] Rows { get; set; }
        private FenIdentifier()
        {

        }
        internal FenIdentifier(string[]rows)
        {
            if (rows.Length!=8)
            {
                throw new InvalidLengthForAFenException();
            }
            Rows=rows;
        }
        public static FenIdentifier Create(string fen)
        {
            return new(fen.Split('/'));
        }
        public static implicit operator string(FenIdentifier fen)
            =>string.Join('/',fen.Rows);
        public static implicit operator FenIdentifier(string fen)
            => new(fen.Split('/'));
        public static implicit operator FenIdentifier(Piece[,]board)
        {
            string[] rows = new string[8];
            for (int i = 0; i < board.GetLength(0); i++)
            {
                int counter = 0;
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] is null)
                    {
                        counter++;
                        continue;
                    }
                    if (counter > 0)
                    {
                        rows[i] += counter.ToString();
                        counter = 0;
                    }
                    rows[i] += board[i, j].Name.Identifier;
                }
                if (counter > 0)
                {
                    rows[i] += counter.ToString();
                }
            }
            return new FenIdentifier(rows);
        }
        public override string ToString()
        {
            return string.Join('/',Rows);
        }
    }
}
