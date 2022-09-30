using Domain.Entities;
using Domain.ValueObjects;
using Shared.Domain;

namespace Domain.Events
{
    public record MakeAMoveBoardDomainEvent(Piece MovedPiece,Piece TakenPiece,PiecePosition StartPosition,PiecePosition EndPosition):IDomainEvent;
    
}
