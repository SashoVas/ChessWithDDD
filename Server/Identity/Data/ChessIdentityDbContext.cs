using Identity.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data
{
    public class ChessIdentityDbContext : IdentityDbContext<User>
    {
        public ChessIdentityDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
