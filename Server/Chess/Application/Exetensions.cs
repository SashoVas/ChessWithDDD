using Application.Commands.Handlers;
using Domain.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class Exetensions
    {
        public static IServiceCollection AddCommandsAndQueries(this IServiceCollection services)
        {
            services.AddSingleton<IBoardFactory, BoardFactory>();
            services.AddSingleton<IPieceFactory, PieceFactory>();
            //services.AddTransient<CreateStandardBoardCommandHandler>();

            return services;
        }
    }
}
