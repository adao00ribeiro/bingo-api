
using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.Interfaces.Services;
using bingo_api.src.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers;



[ApiVersion("1.0")]
public class CardBuyController(ICardBuyService _cardBuyService , TelegamNotifierService _notifier) : ApiControllerBase
{
    private readonly ICardBuyService cardBuyService = _cardBuyService;
    private readonly TelegamNotifierService notifier = _notifier;
    [HttpPost()]
    public async Task<ActionResult<bool>> Buy(CardBuyRequestDto dto)
    {
        try
        {
            throw new Exception("Algo deu errado!");
            return Ok(await this.cardBuyService.Buy(dto));
        }
        catch(Exception ex)
        {
            await notifier.SendMessageAsync($"❌ Erro na API: {ex.Message}");
            return StatusCode(500, "Erro interno");
        }
        
    }
}
