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
public class CardWinnerController(ICardWinnerRepository _cardWinnerRepository) : ApiControllerBase
{
    private readonly ICardWinnerRepository cardWinnerRepository = _cardWinnerRepository;


    [HttpGet()]
    public async Task<ActionResult<IEnumerable<CardWinnerResponseDto>>> GetAll()
    {
        var cardWinners = await cardWinnerRepository.GetAllAsync();
        var cardWinnerResponse = cardWinners.Select(cardWinner => CardWinnerResponseDto.ConvertToDto(cardWinner));
        return Ok(cardWinnerResponse);
    }
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CardWinnerRequestDto request)
    {
        var cardWinner = CardWinnerRequestDto.ConvertToEntity(request);
        var id = await cardWinnerRepository.AddAsync(cardWinner);
        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }
    [HttpGet("id/{id}")]
    public async Task<ActionResult<CardWinnerResponseDto>> GetById(Guid id)
    {
        var cardWinner = await cardWinnerRepository.GetByIdAsync(id);
        if (cardWinner is null)
        {
            return NotFound();
        }
        var userResponse = CardWinnerResponseDto.ConvertToDto(cardWinner);
        return Ok(userResponse);
    }
    [HttpPut]
    public async Task<ActionResult> Update(CardWinnerRequestDto request)
    {
        var cardWinner = CardWinnerRequestDto.ConvertToEntity(request);
        await cardWinnerRepository.UpdateAsync(cardWinner);
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await cardWinnerRepository.RemoveByIdAsync(id);
        return Ok();
    }
}