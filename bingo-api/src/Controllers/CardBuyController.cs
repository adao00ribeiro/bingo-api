
using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers;

[ApiVersion("1.0")]
public class CardBuyController(ICardBuyService _cardBuyService) : ApiControllerBase
{
    private readonly ICardBuyService cardBuyService = _cardBuyService;
    [HttpPost()]
    public async Task<ActionResult<bool>> Buy(CardBuyRequestDto dto)
    {
        return Ok(await this.cardBuyService.Buy(dto));
    }
}
