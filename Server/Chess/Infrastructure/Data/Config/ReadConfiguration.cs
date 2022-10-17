using Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    internal class ReadConfiguration : IEntityTypeConfiguration<BoardReadModel>, IEntityTypeConfiguration<PieceReadModel>, IEntityTypeConfiguration<PieceMovePatternReadModel>
    {
        public void Configure(EntityTypeBuilder<BoardReadModel> builder)
        {
            builder.ToTable("BoardReadModel");
        }

        public void Configure(EntityTypeBuilder<PieceReadModel> builder)
        {
            builder.ToTable("Piece");
        }

        public void Configure(EntityTypeBuilder<PieceMovePatternReadModel> builder)
        {
            builder.ToTable("PieceMovePattern");
        }
    }
}
