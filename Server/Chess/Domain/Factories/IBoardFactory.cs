using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Factories
{
    public interface IBoardFactory
    {
        Board CreateStandardBoard(Guid whitePlayerId,Guid blackPlayerId);
        Board CreateCustomStandard( FenIdentifier fen, Guid whitePlayerId, Guid blackPlayerId, TimeSpan turnDuration, TimeSpan endTurnIncrement);
    }
}
