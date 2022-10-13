using MediatR;

namespace Application.Abstractions
{
    public interface ICommandHandler<T> : IRequestHandler<T> where T : ICommand
    {
    }
}
