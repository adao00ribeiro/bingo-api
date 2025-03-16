using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class PunterMap : IEntityTypeConfiguration<Punter>
{
    public void Configure(EntityTypeBuilder<Punter> builder)
    {
        builder.ToTable("Punters");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.CreateAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.UpdateAt).HasDefaultValueSql("CURRENT_TIMESTAMP"); ;
        builder.Property(x => x.Balance).IsRequired().HasColumnType("numeric(15, 2)");
        builder.Property(x => x.Email).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Cpf).HasMaxLength(11).IsRequired();
        builder.Property(x => x.DateBirth).IsRequired();

        builder.HasMany(p => p.Cards)
           .WithOne(c => c.Punter)
           .HasForeignKey(c => c.PunterId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Recharges)
            .WithOne(r => r.Punter)
            .HasForeignKey(r => r.PunterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Seller)
            .WithMany(s => s.Punters)
            .HasForeignKey(p => p.SellerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
