using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Factories
{
    public interface IBoardFactory
    {
        Board CreateStandardBoard();
        Board CreateCustomStandard( FenIdentifier fen);
    }
}
