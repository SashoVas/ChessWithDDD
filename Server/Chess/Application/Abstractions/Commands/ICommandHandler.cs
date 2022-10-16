using MediatR;

namespace Application.Abstractions.Commands
{
    public interface ICommandHandler<T> : IRequestHandler<T> where T : ICommand
    {
    }
}
