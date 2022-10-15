using Application.Abstractions;

namespace Application.Commands
{
    public sealed record AddPieceCommand(Guid BoardId,Guid UserId,string Name, PiecePositionWriteModel Position, bool IsWhiteColor, List< PieceMoveWriteModel> Moves) :ICommand;
    public sealed record PiecePositionWriteModel(int Row,int Col);
    public sealed record PieceMoveWriteModel(bool IsRepeatable, bool SwapDirections, int RowChange, int ColChange);
}
