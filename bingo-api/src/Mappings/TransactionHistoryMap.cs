using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class TransactionHistoryMap : IEntityTypeConfiguration<TransactionHistory>
{
    public void Configure(EntityTypeBuilder<TransactionHistory> builder)
    {
        builder.ToTable("transaction_histories");  // Plural correto do inglês

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(a => a.EntityId)
               .HasColumnName("entity_id")
               .IsRequired();

        builder.Property(a => a.EntityType)
               .HasColumnName("entity_type")
               .IsRequired();

        builder.Property(a => a.PreviousBalance)
               .HasColumnName("previous_balance")
               .HasColumnType("decimal(15,2)")
               .IsRequired();

        builder.Property(a => a.CurrentBalance)
               .HasColumnName("current_balance")
               .HasColumnType("decimal(15,2)")
               .IsRequired();

        builder.Property(a => a.Amount)
               .HasColumnName("amount")
               .HasColumnType("decimal(15,2)")
               .IsRequired();

        builder.Property(p => p.Type)
               .HasColumnName("type")
               .IsRequired();

        builder.Property(x => x.CreateAt)
     .HasColumnName("create_at")
     .IsRequired()
     .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdateAt)
               .HasColumnName("update_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

    }
}
