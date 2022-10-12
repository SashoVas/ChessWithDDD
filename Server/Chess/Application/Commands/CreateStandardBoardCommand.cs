using MediatR;

namespace Application.Commands
{
    public sealed record CreateStandardBoardCommand(Guid WhitePlayerId,Guid BlackPlayerId):IRequest;
}
