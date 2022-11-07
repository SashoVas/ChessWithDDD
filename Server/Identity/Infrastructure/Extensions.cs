using Identity.Data;
using Identity.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure
{
    public static class Extensions
    {
        public static void AddDB(this IServiceCollection services,IConfiguration configuration)
        {
            var defaultConnection = configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
            services.AddDbContext<ChessIdentityDbContext>(options => options.UseSqlServer(defaultConnection));
        }

        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options => {
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;
            })
            .AddEntityFrameworkStores<ChessIdentityDbContext>()
            .AddDefaultTokenProviders();
        }

        public static void EnsureDbsAreCreated(this IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetRequiredService<ChessIdentityDbContext>())
                {
                    context.Database.EnsureCreated();
                }
            }
        }
    }
}
