namespace Domain.ValueObjects
{
    public record PieceName
    {
        public string Name { get; }
        public string Identifier { get; }

        public PieceName(string name,PieceColor pieceColor,string identifier=null)
        {
            if (name == null)
            {
                throw new ArgumentNullException("Name should not be null");
            }
            Name =name;
            if (identifier is null)
            {
                if (pieceColor==PieceColor.White)
                {
                    Identifier = Name[0].ToString().ToUpper();
                }
                else
                {
                    Identifier = Name[0].ToString();
                }
            }
            else
            {
                if (pieceColor == PieceColor.White)
                {
                    Identifier = identifier.ToString().ToUpper();
                }
                else
                {
                    Identifier = identifier.ToString();
                }
            }
        }
        public static implicit operator string(PieceName name) 
            => name.Name;
    }
}
