using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using bingo_api.src.Entities;

namespace bingo_api.src.Mappings;

public class AccumulatedMap : IEntityTypeConfiguration<Accumulated>
{
    public void Configure(EntityTypeBuilder<Accumulated> builder)
    {
        builder.ToTable("accumulateds");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(a => a.Activated)
               .HasColumnName("activated")
               .IsRequired();

        builder.Property(a => a.MinimumValue)
               .HasColumnName("minimum_value")
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(a => a.MaximumValue)
               .HasColumnName("maximum_value")
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(a => a.CurrentValue)
               .HasColumnName("current_value")
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(a => a.MaximumNumberOfBalls)
               .HasColumnName("maximum_number_of_balls")
               .IsRequired();

        builder.Property(a => a.CumulativePercentage)
               .HasColumnName("cumulative_percentage")
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(a => a.IncrementBallCumulative)
               .HasColumnName("increment_ball_cumulative")
               .IsRequired();

        builder.Property(x => x.CreatedAt)
             .HasColumnName("created_at")
             .IsRequired()
             .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
               .HasColumnName("updated_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Relacionamento com Room (um-para-um)
        builder.HasOne(a => a.Room)
               .WithOne(r => r.Accumulated)
               .HasForeignKey<Accumulated>(a => a.RoomId)
               .HasConstraintName("fk_accumulated_room_id")
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();

        builder.Property(a => a.RoomId)
               .HasColumnName("room_id");
    }
}
