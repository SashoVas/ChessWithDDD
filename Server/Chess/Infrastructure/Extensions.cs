using Application.Services;
using Domain.Repositories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddServicesAndRepositories(this IServiceCollection services)
        {
            services.AddTransient<IBoardService, BoardService>();
            services.AddTransient<IChessRepository, ChessRepository>();
            return services;
        }
    }
}
