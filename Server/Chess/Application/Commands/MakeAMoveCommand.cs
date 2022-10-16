using Application.Abstractions.Commands;

namespace Application.Commands
{
    public sealed record MakeAMoveCommand(Guid BoardId,Guid PlayerId,int StartRow,int StartCol,int EndRow,int EndCol):ICommand;

}
