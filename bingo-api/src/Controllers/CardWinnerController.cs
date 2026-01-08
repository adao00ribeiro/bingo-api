using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using bingo_api.src.Entities;
using bingo_api.src.Constants;
using Microsoft.EntityFrameworkCore;
using bingo_api.src.DTOs.Response.report;
namespace bingo_api.src.Controllers;

[Authorize]

[ApiVersion("1.0")]
public class CardWinnerController(ICardWinnerRepository cardWinnerRepository) : ApiControllerBase
{
    private readonly ICardWinnerRepository _cardWinnerRepository = cardWinnerRepository;

    [Authorize(Roles = $"{Roles.Admin},{Roles.Punter}")]
    [HttpGet()]
    public async Task<ActionResult<ReportResponseDto<CardWinnerResponseDto, object>>> GetAll(int? page = null, int? size = null)
    {

        var entityId = User.FindFirst("entityid")?.Value;
        int totalCount;
        IEnumerable<CardWinner> cardWinners;

        if (User.IsInRole(Roles.Admin))
        {
            // Se for Admin, retorna todas as recargas
            totalCount = await _cardWinnerRepository.CountAsync();
            cardWinners = await _cardWinnerRepository.GetAllAsync(page, size);
        }
        else if (User.IsInRole(Roles.Punter) && Guid.TryParse(entityId, out _))
        {
            totalCount = await _cardWinnerRepository.CountAsync(Guid.Parse(entityId));
            cardWinners = await _cardWinnerRepository.GetAllAsync(page, size,
                filter: r => r.Card.PunterId == Guid.Parse(entityId),
                includeProperties: q => q.Include(x => x.Prize).ThenInclude(x => x.Round).Include(x => x.Card));
        }
        else
        {
            return Forbid(); // Bloqueia caso o usuário não seja admin nem punter
        }
        var cardWinnerDtos = cardWinners.Select(r => CardWinnerResponseDto.ConvertToDto(r)).ToList();

        // Paginação simples
        var pageNumber = page ?? 1;
        var pageSize = size ?? cardWinnerDtos.Count;
        var pagedRows = cardWinnerDtos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        var response = new ReportResponseDto<CardWinnerResponseDto, object>
        {
            Rows = pagedRows,
            Stats = null,                  // opcional, você pode criar um objeto de estatísticas se quiser
            StartingOn = null,
            EndingOn = null,
            Page = pageNumber,
            PerPage = pageSize,
            RowsCount = cardWinnerDtos.Count
        };

        return Ok(response);
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