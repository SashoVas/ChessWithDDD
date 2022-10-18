using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.Config
{
    internal class WriteConfiguration : IEntityTypeConfiguration<Board>, IEntityTypeConfiguration<Piece>, IEntityTypeConfiguration<PieceMovePattern>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            var fenConverter = new ValueConverter<FenIdentifier,string>(l=>l.ToString(),l=>FenIdentifier.Create(l));

            builder.Property(b => b.Fen)
                .HasConversion(fenConverter)
                .HasColumnName("Fen");

            builder.HasMany(b => b.Pieces);
            builder.Property(b => b.WhitePlayerId);
            builder.Property(b => b.BlackPlayerId);
            builder.Property(b => b.IsWhiteOnTurn);

            builder.ToTable("Board");
        }
        public void Configure(EntityTypeBuilder<Piece> builder)
        {
            builder.Property(p => p.Id);
            builder.Property(p=>p.Position.Row);
            builder.Property(p=>p.Position.Col);
            builder.Property(p=>p.Name.Name);
            builder.Property(p=>p.Name.Identifier);
            builder.Property(p=>p.IsTaken);
            builder.Property(p=>p.Color);
            builder.Property<Guid>("BoardId");
            builder.HasMany(p => p.Moves);

            builder.ToTable("Piece");
        }
        public void Configure(EntityTypeBuilder<PieceMovePattern> builder)
        {
            builder.Property<Guid>("Id");
            builder.Property<Guid>("PieceReadModelId");
            builder.Property(p => p.IsRepeatable);
            builder.Property(p => p.SwapDirections);
            builder.Property(p => p.ColChange);
            builder.Property(p => p.RowChange);

            builder.ToTable("PieceReadModel");
        }        
    }
}
