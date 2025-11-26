using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class SellerWithdrawalMap : IEntityTypeConfiguration<SellerWithdrawal>
{
    public void Configure(EntityTypeBuilder<SellerWithdrawal> builder)
    {
        builder.HasOne(sw => sw.Seller)
            .WithMany(s => s.Withdrawals)
            .HasForeignKey(sw => sw.SellerId);
    }
}