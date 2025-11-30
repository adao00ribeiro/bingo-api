using bingo_api.src.Entities;
using bingo_api.src.Enums;

namespace bingo_api.src.DTOs.Response;

public record SellerWithdrawalResponseDto : WithdrawalResponseDto
{
    public Guid SellerId { get; set; }
    public Seller Seller{ get; set; }

    public SellerWithdrawalResponseDto(Guid id,string withdrawalType, decimal amount, EPaymentStatus status, DateTime? confirmedAt, DateTime createdAt, DateTime updatedAt, Guid sellerId , Seller seller)
   : base(id,withdrawalType, amount, status, confirmedAt, createdAt, updatedAt)
    {
        SellerId = sellerId;
        Seller = seller;
    }
    public static SellerWithdrawalResponseDto ConvertToDto(SellerWithdrawal w)
    {
        return new SellerWithdrawalResponseDto(w.Id,w.WithdrawalType, w.Amount, w.Status, w.ConfirmedAt, w.CreatedAt, w.UpdatedAt, w.SellerId , w.Seller);
    }
}
