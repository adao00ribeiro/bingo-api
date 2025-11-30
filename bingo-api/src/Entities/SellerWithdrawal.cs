using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities;

public class SellerWithdrawal : Withdrawal
{
    public Guid SellerId { get; set; }
    public Seller Seller { get; set; } = null!;
}
