using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data
{
    public static class Extensions
    {
        public static IServiceCollection AddContexts(this IServiceCollection services,IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
            //services.AddDbContext<ReadDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDbContext<WriteDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
        public static void EnsureDbsAreCreated(this IServiceScope serviceScope)
        {
            //using (var context = serviceScope.ServiceProvider.GetRequiredService<ReadDbContext>())
            //{
            //    context.Database.EnsureCreated();
            //}
            using (var context = serviceScope.ServiceProvider.GetRequiredService<WriteDbContext>())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
