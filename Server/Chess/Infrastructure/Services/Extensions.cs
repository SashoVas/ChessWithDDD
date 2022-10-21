using Application.Services;
using Domain.Repositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services
{
    public static class Extensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IBoardService, BoardService>();
            services.AddTransient<IChessRepository, ChessRepository>();
            return services;
        }
    }
}
