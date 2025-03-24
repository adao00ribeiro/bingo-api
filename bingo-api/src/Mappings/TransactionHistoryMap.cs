using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class TransactionHistoryMap : IEntityTypeConfiguration<TransactionHistory>
{
    public void Configure(EntityTypeBuilder<TransactionHistory> builder)
    {
        builder.ToTable("TransactionHistorys");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
        .ValueGeneratedOnAdd()

        ;
        builder.Property(a => a.EntityType)
              .IsRequired();

        builder.Property(a => a.PreviousBalance)
               .HasColumnType("decimal(15,2)")
               .IsRequired();

        builder.Property(a => a.CurrentBalance)
               .HasColumnType("decimal(15,2)")
               .IsRequired();

        builder.Property(a => a.Amount)
       .HasColumnType("decimal(15,2)")
       .IsRequired();
       
        builder.Property(p => p.Type)
        .IsRequired();
    }
}
