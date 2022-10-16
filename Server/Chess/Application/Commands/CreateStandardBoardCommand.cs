using Application.Abstractions.Commands;

namespace Application.Commands
{
    public sealed record CreateStandardBoardCommand(Guid WhitePlayerId,Guid BlackPlayerId): ICommand;
}
