using MediatR;

namespace Application.Abstractions.Queries
{
    public interface IQuery<T>:IRequest<T>
    {
    }
}
