using Application.Commands.Handlers;
using Domain.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class Exetensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddSingleton<IBoardFactory, BoardFactory>();
            services.AddSingleton<IPieceFactory, PieceFactory>();

            return services;
        }
    }
}
