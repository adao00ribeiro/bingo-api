using bingo_api.src.Entities.Blockchain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings.Blockchain;

public class NetworkMap : IEntityTypeConfiguration<Network>
{
    public void Configure(EntityTypeBuilder<Network> builder)
    {
        builder.ToTable("blockchain_networks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
               .HasColumnName("name")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.RpcUrl)
               .HasColumnName("rpc_url")
               .IsRequired();

        builder.Property(x => x.ChainId)
               .HasColumnName("chain_id")
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
        builder.HasMany(n => n.TokenAddresses)
               .WithOne(ta => ta.Network)
               .HasForeignKey(ta => ta.NetworkId)
               .HasConstraintName("fk_blockchain_token_addresses_network_id")
               .OnDelete(DeleteBehavior.Cascade);
    }
}