using Domain.Entities;
using Domain.Factories;
using Domain.ValueObjects;
using Shared.Domain;

namespace Domain.Policies
{
    internal sealed class CustomStandardBoardPolicy : IBoardPolicy
    {
        public bool IsAplicable(FenIdentifier fen) 
            => fen != DomainConstants.DefaultBoardStartPositionFen;
        public IEnumerable<Piece> GenerateItems(IPieceFactory pieceFactory, FenIdentifier fen)
        {
            var pieces=new List<Piece>();
            for (int i = 0; i < fen.Rows.Length; i++)
            {
                int col = 0;
                foreach (var letter in fen.Rows[i])
                {   
                    if (int.TryParse(letter.ToString(),out int empty) )
                    {
                        col += empty;
                    }
                    var color = PieceColor.Black;
                    if (char.IsUpper(letter))
                    {
                        color = PieceColor.White;
                    }
                    var position = new PiecePosition(i, col);
                    switch (letter)
                    {
                        case 'k':
                            pieces.Add(pieceFactory.CreateKing(position, color));
                            break;
                        case 'q':
                            pieces.Add(pieceFactory.CreateQueen(position, color));
                            break;
                        case 'b':
                            pieces.Add(pieceFactory.CreateBishop(position, color));
                            break;
                        case 'r':
                            pieces.Add(pieceFactory.CreateRook(position, color));
                            break;
                        case 'p':
                            pieces.Add(pieceFactory.CreatePawn(position, color));
                            break;
                        case 'n':
                            pieces.Add(pieceFactory.CreateKnight(position, color));
                            break;
                        default:
                            throw new Exception("No such figure");
                    }
                    col++;
                }
            }
            return pieces;
        }
    }
}
