﻿using Domain.Entities;

namespace Domain.ValueObjects
{
    public record FenIdentifier
    {
        public string[] Rows { get; set; }
        public FenIdentifier(string[]rows)
        {
            if (rows.Length!=8)
            {
                throw new Exception("Infvalid board fen");
            }
            Rows=rows;
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
    }
}