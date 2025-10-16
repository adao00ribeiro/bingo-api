using bingo_api.src.Structs;

namespace bingo_api.src.DTOs.Request;

public record SellerPatchRequestDto
{
    public SellerSettings Settings { get; set; }
}
