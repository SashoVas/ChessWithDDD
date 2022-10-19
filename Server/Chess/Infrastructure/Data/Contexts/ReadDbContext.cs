using Infrastructure.Data.Config;
using Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Contexts
{
    internal class ReadDbContext : DbContext
    {
        public DbSet<BoardReadModel> Board { get; set; }
        public DbSet<PieceReadModel> Pieces { get; set; }
        public DbSet<PieceMovePatternReadModel> Moves { get; set; }
        public ReadDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BoardReadModel>()
                .HasMany(b => b.Pieces)
                .WithOne(p => p.Board)
                .HasForeignKey(p => p.BoardId);

            builder.Entity<PieceMovePatternReadModel>()
                .HasOne(pm => pm.Piece)
                .WithMany(p => p.Moves)
                .HasForeignKey(pm => pm.PieceId);


            //builder.HasDefaultSchema("chess");
            //var readModelConfig = new ReadConfiguration();
            //builder.ApplyConfiguration<BoardReadModel>(readModelConfig);
            //builder.ApplyConfiguration<PieceReadModel>(readModelConfig);
            //builder.ApplyConfiguration<PieceMovePatternReadModel>(readModelConfig);
        }
    }
}
