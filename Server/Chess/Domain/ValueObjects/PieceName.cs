namespace Domain.ValueObjects
{
    public record PieceName
    {
        public string Name { get; }
        public string Identifier { get; }

        public PieceName(string name,PieceColor pieceColor)
        {
            if (Name == null)
            {
                throw new ArgumentNullException("Name should not be null");
            }
            Name =name;
            
            if (pieceColor==PieceColor.White)
            {
                Identifier = Name[0].ToString().ToUpper();
            }
            else
            {
                Identifier = Name[0].ToString();
            }
        }
        public static implicit operator string(PieceName name) 
            => name.Name;
        public static implicit operator PieceName(string name)
            => new(name);
    }
}
