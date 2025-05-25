using System.Security.Claims;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
namespace bingo_api.src.Controllers;
[Authorize]

[ApiVersion("1.0")]
public class CardController(ICardRepository _cardRepository, IPunterRepository _punterRepository) : ApiControllerBase
{
    private readonly ICardRepository cardRepository = _cardRepository;
    private readonly IPunterRepository punterRepository = _punterRepository;


    [HttpGet()]
    public async Task<ActionResult<IEnumerable<CardResponseDto>>> GetAll()
    {
        var identity = User.Identity as ClaimsIdentity;
        var UserId = identity?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        foreach (var claim in identity.Claims)
        {
            Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
        }
        if (string.IsNullOrEmpty(UserId))
        {
            return Unauthorized(new { message = "Usuário não autenticado." });
        }

        var cards = await cardRepository.GetAllAsync();
        var cardsUser = cards.Where(x => x.PunterId.ToString() == UserId);
        var cardsResponse = cards.Select(c => CardResponseDto.ConvertToDto(c));
        return Ok(cardsResponse);
    }
    [HttpGet("round/{roundId}")]
    public async Task<ActionResult<PagedResponseDto<CardResponseDto>>> GetAllByRoundId(Guid roundId, int? page = null, int? size = null)
    {
        var identity = User.Identity as ClaimsIdentity;
        var userEmail = identity?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized(new { message = "Usuário não autenticado." });
        }
        var punter = await punterRepository.GetByEmailAsync(userEmail);
        if (punter is null)
        {
            return BadRequest();
        }
        var cards = await cardRepository.GetAllByRoundId(punter.Id, roundId, page, size, includeProperties: c => c.Round);
        int totalCount = cards.Count();
        var cardsResponse = cards.Select(c => CardResponseDto.ConvertToDto(c));
        return Ok(new PagedResponseDto<CardResponseDto>
        {
            Items = cardsResponse,
            TotalCount = totalCount
        });
    }

    [HttpGet("id/{id}")]
    public async Task<ActionResult<CardResponseDto>> GetById(Guid id)
    {
        var card = await cardRepository.GetByIdAsync(id);
        return Ok(CardResponseDto.ConvertToDto(card));
    }
    [HttpPut]
    public async Task<ActionResult> Update(CardRequestDto request)
    {
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await cardRepository.RemoveByIdAsync(id);
        return Ok();
    }
}
