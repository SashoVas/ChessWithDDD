using Infrastructure.Data.Config;
using Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Contexts
{
    internal class ReadDbContext : DbContext
    {
        public DbSet<BoardReadModel> Board { get; set; }
        public ReadDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readModelConfig = new ReadConfiguration();
            builder.ApplyConfiguration<BoardReadModel>(readModelConfig);
            builder.ApplyConfiguration<PieceReadModel>(readModelConfig);
            builder.ApplyConfiguration<PieceMovePatternReadModel>(readModelConfig);
        }
    }
}
