using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Factories
{
    public interface IBoardFactoriy
    {
        Board CreateStandardBoard();
        Board CreateCustomStandard(FenIdentifier fen);
    }
}
