using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Policies
{
    public interface IBoardPolicy
    {
        bool IsAplicable(FenIdentifier fen);
        IEnumerable<Piece> GenerateItems(FenIdentifier fen);
    }
}
