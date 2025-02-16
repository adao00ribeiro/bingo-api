using bingo_api.src.DTOs.Request;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Interfaces.Services;

public interface ICardBuyService
{
    Task<bool> Buy(CardBuyRequestDto dto);
}
