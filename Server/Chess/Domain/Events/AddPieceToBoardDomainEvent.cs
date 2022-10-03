using Domain.Entities;
using Shared.Domain;

namespace Domain.Events
{
    public record AddPieceToBoardDomainEvent(Piece Piece): IDomainEvent;

}
