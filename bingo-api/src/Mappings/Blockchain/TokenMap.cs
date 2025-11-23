using bingo_api.src.Entities.Blockchain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings.Blockchain;

public class TokenMap: IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.ToTable("blockchain_tokens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(x => x.Symbol)
               .HasColumnName("symbol")
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(x => x.Name)
               .HasColumnName("name")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Decimals)
               .HasColumnName("decimals")
               .IsRequired();

       builder.Property(x => x.IsNative)
               .HasColumnName("is_native")
               .IsRequired();

        builder.Property(x => x.CreatedAt)
               .HasColumnName("created_at")
               .IsRequired()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
               .HasColumnName("updated_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

         builder.Property(x => x.DiscardedAt)
               .HasColumnName("discarded_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Relacionamento 1:N com TokenAddresses
          builder.HasMany(t => t.TokenAddresses)
               .WithOne(ta => ta.Token)
               .HasForeignKey(ta => ta.TokenId)
               .HasConstraintName("fk_blockchain_token_addresses_token_id")
               .OnDelete(DeleteBehavior.Cascade);
    }
}