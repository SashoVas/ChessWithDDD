using Domain.Entities;
using Domain.Factories;
using Domain.ValueObjects;

namespace Domain.Policies
{
    internal interface IBoardPolicy
    {
        bool IsAplicable(FenIdentifier fen);
        IEnumerable<Piece> GenerateItems(IPieceFactory pieceFactory, FenIdentifier fen);
    }
}
