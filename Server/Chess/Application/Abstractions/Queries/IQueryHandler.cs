using MediatR;

namespace Application.Abstractions.Queries
{
    internal interface IQueryHandler<TQuery, TQueryOutput> : IRequestHandler<TQuery, TQueryOutput> where TQuery : IQuery<TQueryOutput>
    {
    }
}
