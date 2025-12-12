using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using bingo_api.src.DTOs.Response.report;
using bingo_api.src.Entities;
using bingo_api.src.Services;
namespace bingo_api.src.Controllers;


[Authorize]
[ApiVersion("1.0")]
public class RoundController(IRoundRepository _roundRepository, IPunterRepository _punterRepository, MinioFileService _minioFileService) : ApiControllerBase
{
    private readonly IRoundRepository roundRepository = _roundRepository;
    private readonly IPunterRepository punterRepository = _punterRepository;
    private readonly MinioFileService minioFileService = _minioFileService;


    [HttpGet()]

    public async Task<ActionResult<ReportResponseDto<RoundResponseDto, object>>> GetAll(
        int? page = null,
        int? size = null)
    {
        var rounds = await roundRepository.GetAllAsync(page, size, includeProperties: q => q.Include(x => x.Prizes));
        var roundDtos = rounds.Select(r => RoundResponseDto.ConvertToDto(r)).ToList();
        var totalCount = await roundRepository.CountAsync();
        var response = new ReportResponseDto<RoundResponseDto, object>
        {
            Rows = roundDtos,
            Stats = null,
            StartingOn = null,
            EndingOn = null,
            Page = page,
            PerPage = size,
            RowsCount = totalCount
        };
        return Ok(response);
    }
    [HttpGet("next")]
    public async Task<ActionResult<ReportResponseDto<RoundResponseDto, object>>> GetNextRounds(
      int? page = null,
      int? size = null)
    {
        var entityId = User.FindFirst("entityid")?.Value;

        var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        if (string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(userRole))
        {
            return Unauthorized("Usuário não autenticado.");
        }
        IEnumerable<Round> rounds = new List<Round>();
        if (User.IsInRole("Punter"))
        {

            var punter = await punterRepository.GetByIdAsync(Guid.Parse(entityId));

            if (punter is null)
            {
                return Forbid();
            }
            // Punter pode ver apenas as salas dos Sellers associados a ele
            rounds = await roundRepository.GetNextRoundsAsync(page, size, punter.SellerId);
        }
        else
        {
            return Forbid();
        }
        

        var roundDtos = rounds.Select(r => RoundResponseDto.ConvertToDto(r)).ToList();
        var response = new ReportResponseDto<RoundResponseDto, object>
        {
            Rows = roundDtos,
            Stats = null,
            StartingOn = null,
            EndingOn = null,
            Page = page,
            PerPage = size,
            RowsCount = roundDtos.Count
        };
        return Ok(response);
    }
    [HttpGet("filter/room/{id}")]
    public async Task<ActionResult<IEnumerable<RoundResponseDto>>> FilterByRoomIdAsync(Guid id)
    {

        var identity = User.Identity as ClaimsIdentity;
        var userEmail = identity?.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized(new { message = "Usuário não autenticado." });
        }

        var punter = await this.punterRepository.GetByEmailAsync(userEmail);

        if (punter is null)
        {
            return NotFound();
        }
       //     await Task.Delay(TimeSpan.FromSeconds(15));
        var rounds = await roundRepository.FilterByRoomIdAsync(id, punter.Id);

        var roundsResponse = rounds.Select(r => RoundResponseDto.ConvertToDto(r));
        return Ok(roundsResponse);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(RoundRequestDto request)
    {

        var round = RoundRequestDto.ConvertToEntity(request);
        var id = await roundRepository.AddAsync(round);
        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }
    [HttpPost("bulk")]
    public async Task<ActionResult<bool>> CreateBulk(RoundBulkRequestDto dto)
    {
        return Ok(await roundRepository.GenerateRounds(dto));
    }
    [HttpGet("id/{id}")]
    public async Task<ActionResult<RoundResponseDto>> GetById(Guid id)
    {
        var round = await roundRepository.GetByIdAsync(id);

        if (round is null)
        {
            return NotFound();
        }
        round.Prizes = await roundRepository.GetPrizes(id);
        return Ok(RoundResponseDto.ConvertToDto(round));
    }

    [HttpPut]
    public async Task<ActionResult> Update(RoundRequestDto request)
    {
        var round = RoundRequestDto.ConvertToEntity(request);
        await roundRepository.UpdateAsync(round);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await roundRepository.RemoveByIdAsync(id);
        return Ok();
    }


}