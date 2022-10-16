using Application.Abstractions.Queries;
using Application.DTO;

namespace Application.Queries
{
    public record GetBoardQuery(Guid BoardId,Guid CallerId):IQuery<BoardDTO>;
}
