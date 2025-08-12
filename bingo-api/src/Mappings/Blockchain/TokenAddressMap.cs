using bingo_api.src.Entities.Blockchain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings.Blockchain;

public class TokenAddressMap: IEntityTypeConfiguration<TokenAddress>
{
    public void Configure(EntityTypeBuilder<TokenAddress> builder)
    {
       builder.ToTable("blockchain_token_addresses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(x => x.ContractAddress)
               .HasColumnName("contract_address")
               .HasMaxLength(42)
               .IsRequired();

        builder.Property(x => x.TokenId)
               .HasColumnName("token_id")
               .IsRequired();

        builder.Property(x => x.NetworkId)
               .HasColumnName("network_id")
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

        builder.HasOne(ta => ta.Token)
               .WithMany(t => t.TokenAddresses)
               .HasForeignKey(ta => ta.TokenId)
               .HasConstraintName("fk_blockchain_token_addresses_token_id")
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();

        builder.HasOne(ta => ta.Network)
               .WithMany(n => n.TokenAddresses)
               .HasForeignKey(ta => ta.NetworkId)
               .HasConstraintName("fk_blockchain_token_addresses_network_id")
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();

        builder.HasIndex(x => new { x.TokenId, x.NetworkId })
               .IsUnique()
               .HasDatabaseName("ux_blockchain_token_addresses_token_network");
    }
}