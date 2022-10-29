using Domain.Exceptions;
using Shared.Domain;

namespace Domain.ValueObjects
{
    public record PieceName
    {
        public string Name { get; }
        public string Identifier { get; }
        private PieceName()
        {

        }
        public PieceName(string name,PieceColor pieceColor,string identifier=null)
        {
            if (name == null || name.Length< DomainConstants.MinNameLength|| name.Length>DomainConstants.MaxNameLength)
            {
                throw new InvalidPieceNameException(name);
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
