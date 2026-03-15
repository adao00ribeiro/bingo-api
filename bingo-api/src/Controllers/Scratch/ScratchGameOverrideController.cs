using Asp.Versioning;
using bingo_api.src.Constants;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request.Scratch;
using bingo_api.src.DTOs.Response.report;
using bingo_api.src.DTOs.Response.Scratch;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Interfaces.Repositories.Scratch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Controllers.Scratch;

[Authorize]
[ApiVersion("1.0")]
public class ScratchGameOverrideController(
    IScratchGameOverrideRepository scratchGameOverrideRepository,
        IPunterRepository punterRepository

    ) : ApiControllerBase
{
    private readonly IScratchGameOverrideRepository _scratchGameOverrideRepository = scratchGameOverrideRepository;
    private readonly IPunterRepository _punterRepository = punterRepository;

    [HttpGet]
    public async Task<ActionResult<ReportResponseDto<ScratchGameOverrideResponseDto, object>>> GetAll(
     int? page = null, int? size = null)
    {

        var entityId = User.FindFirst("entityid")?.Value;
        int totalCount;
        IEnumerable<ScratchGameOverride> ScratchGameOverrides;
        if (User.IsInRole(Roles.Admin))
        {
            // Se for Admin, retorna todas as recargas
            totalCount = await _scratchGameOverrideRepository.CountAsync();
            ScratchGameOverrides = await _scratchGameOverrideRepository.GetAllAsync(page, size, includeProperties: x => x.Include(x => x.ScratchGame));
        }
        else if (User.IsInRole(Roles.Punter) && Guid.TryParse(entityId, out _))
        {
            var punter = await _punterRepository.GetByIdAsync(Guid.Parse(entityId));
            if (punter is null)
            {
                throw new Exception("Usuário não encontrado");
            }
            totalCount = await _scratchGameOverrideRepository.CountAsync(punter.OnlineHouseId);
            ScratchGameOverrides = await _scratchGameOverrideRepository.GetAllAsync(page, size, filter: r => r.OnlineHouseId == punter.OnlineHouseId,
                    includeProperties: x => x.Include(x => x.ScratchGame)
                );
        }
        else
        {
            Console.WriteLine("bloqueado");
            return Forbid(); // Bloqueia caso o usuário não seja admin nem punter
        }

        var sellerGamesDtos = ScratchGameOverrides.Select(r => ScratchGameOverrideResponseDto.ConvertToDto(r)).ToList();

        var pageNumber = page ?? 1;
        var pageSize = size ?? sellerGamesDtos.Count;
        var pagedRows = sellerGamesDtos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        var response = new ReportResponseDto<ScratchGameOverrideResponseDto, object>
        {
            Rows = pagedRows,
            Stats = null,
            StartingOn = null,
            EndingOn = null,
            Page = pageNumber,
            PerPage = pageSize,
            RowsCount = sellerGamesDtos.Count
        };

        return Ok(response);

    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(ScratchGameOverrideRequestDto request)
    {

        var scratchSellerGame = ScratchGameOverrideRequestDto.ConvertToEntity(request);
        var id = await _scratchGameOverrideRepository.AddAsync(scratchSellerGame);
        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }

    [HttpGet("id/{id}")]
    public async Task<ActionResult<ScratchGameOverrideResponseDto>> GetById(Guid id)
    {
        var scratchSellerGame = await _scratchGameOverrideRepository.GetByIdAsync(id);

        if (scratchSellerGame is null)
        {
            return NotFound();
        }

        return Ok(ScratchGameOverrideResponseDto.ConvertToDto(scratchSellerGame));
    }
}
