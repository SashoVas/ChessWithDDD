using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    internal class WriteConfiguration : IEntityTypeConfiguration<Board>, IEntityTypeConfiguration<Piece>, IEntityTypeConfiguration<PieceMovePattern>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            //var fenConverter = new ValueConverter<FenIdentifier,string>(l=>l.ToString(),l=>FenIdentifier.Create(l));

            builder.Property(b => b.Fen)
                .HasConversion(f=>f.ToString(),f=>FenIdentifier.Create(f))
                .HasColumnName("Fen");

            builder.HasMany(b => b.Pieces);
            builder.Property(b => b.WhitePlayerId);
            builder.Property(b => b.BlackPlayerId);
            builder.Property(b => b.IsWhiteOnTurn);

            builder.ToTable("Boards");
        }
        public void Configure(EntityTypeBuilder<Piece> builder)
        {
            builder.Property(p => p.Id);
            //builder.Property(p=>p.Position.Row);
            //builder.Property(p=>p.Position.Col);
            builder.OwnsOne(p => p.Position, position =>
                {
                    position.Property(r => r.Row).HasColumnName("Row");
                    position.Property(c => c.Col).HasColumnName("Col");
                }            
            );
            //builder.Property(p=>p.Name.Name);
            //builder.Property(p=>p.Name.Identifier);
            builder.OwnsOne(p => p.Name, name =>
                {
                    name.Property(r => r.Name).HasColumnName("Name");
                    name.Property(c => c.Identifier).HasColumnName("Identifier");
                }
            );
            builder.Property(p=>p.IsTaken);
            builder.Property(p=>p.Color);
            builder.Property<Guid>("BoardId");
            builder.HasMany(p => p.Moves);

            builder.ToTable("Pieces");
        }
        public void Configure(EntityTypeBuilder<PieceMovePattern> builder)
        {
            builder.Property<Guid>("Id");
            //builder.HasKey("Id");
            builder.Property<Guid>("PieceId");
            builder.Property(p => p.IsRepeatable);
            builder.Property(p => p.SwapDirections);
            builder.Property(p => p.ColChange);
            builder.Property(p => p.RowChange);

            builder.ToTable("Moves");
        }        
    }
}
