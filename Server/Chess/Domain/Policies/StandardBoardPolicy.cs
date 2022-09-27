using Domain.Entities;
using Domain.Factories;
using Domain.ValueObjects;
using Shared.Domain;

namespace Domain.Policies
{
    internal sealed class StandardBoardPolicy
    {
        private readonly IPieceFactory pieceFactory;
        public StandardBoardPolicy(IPieceFactory pieceFactory) 
            => this.pieceFactory = pieceFactory;
        public bool IsAplicable(FenIdentifier fen) => fen == DomainConstants.DefaultBoardStartPositionFen;
        public IEnumerable<Piece> GenerateItems(FenIdentifier fen)
        {
            var pieces=new List<Piece>();
            var color =PieceColor.White;
            for (int row = 0; row <= 7; row+=7)
            {
                pieces.Add(pieceFactory.CreateRook(new(row,0),color));
                pieces.Add(pieceFactory.CreateRook(new(row, 7), color));

                pieces.Add(pieceFactory.CreateKnight(new(row, 1),color));
                pieces.Add(pieceFactory.CreateKnight(new(row, 6), color));

                pieces.Add(pieceFactory.CreateBishop(new(row,2),color));
                pieces.Add(pieceFactory.CreateBishop(new(row, 5), color));

                pieces.Add(pieceFactory.CreateQueen(new(row, 3), color));
                pieces.Add(pieceFactory.CreateKing(new(row, 4), color));
                color=PieceColor.Black;
            }
            color = PieceColor.White;
            for (int row = 1; row < 8; row+=5)
            {
                for (int col = 0; col < 8; col++)
                {
                    pieces.Add(pieceFactory.CreatePawn(new(row,col),color));
                }
                color = PieceColor.Black;
            }
            return pieces;
        }
    }
}
