using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Contexts
{
    internal class WriteDbContext : DbContext
    {
        public DbSet<Board> Boards { get; set; }
        public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var writeConfig = new WriteConfiguration();
            builder.ApplyConfiguration<Board>(writeConfig);
            builder.ApplyConfiguration<Piece>(writeConfig);
            builder.ApplyConfiguration<PieceMovePattern>(writeConfig);
        }
    }
}
