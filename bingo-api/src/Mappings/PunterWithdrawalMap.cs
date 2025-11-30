using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class PunterWithdrawalMap : IEntityTypeConfiguration<PunterWithdrawal>
{
    public void Configure(EntityTypeBuilder<PunterWithdrawal> builder)
    {
        builder.HasOne(pw => pw.Punter)
            .WithMany(p => p.Withdrawals)
            .HasForeignKey(pw => pw.PunterId);
    }
}