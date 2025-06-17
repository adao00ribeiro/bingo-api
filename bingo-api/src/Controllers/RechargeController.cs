using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using bingo_api.src.Constants;
using bingo_api.src.Entities;
namespace bingo_api.src.Controllers;


[Authorize]

[ApiVersion("1.0")]
public class RechargeController(IRechargeRepository _rechargeRepository, ISellerRepository _sellerRepository) : ApiControllerBase
{
    private readonly IRechargeRepository rechargeRepository = _rechargeRepository;
    private readonly ISellerRepository sellerRepository = _sellerRepository;

    [Authorize(Roles = $"{Roles.Admin},{Roles.Punter}")]
    [HttpGet()]
    public async Task<ActionResult<PagedResponseDto<RechargeResponseDto>>> GetAll(int? page = null, int? size = null)
    {
        var entityId = User.FindFirst("entityid")?.Value;
        int totalCount;
        IEnumerable<Recharge> recharges;

        if (User.IsInRole(Roles.Admin))
        {
            // Se for Admin, retorna todas as recargas
            totalCount = await rechargeRepository.CountAsync();
            recharges = await rechargeRepository.GetAllAsync(page, size, includeProperties: r => r.Punter);
        }
        else if (User.IsInRole(Roles.Punter) && Guid.TryParse(entityId, out _))
        {
            totalCount = await rechargeRepository.CountAsync(Guid.Parse(entityId));
            recharges = await rechargeRepository.GetAllAsync(page, size,
                filter: r => r.PunterId == Guid.Parse(entityId),
                includeProperties: r => r.Punter);
        }
        else
        {
            Console.WriteLine("bloqueado");
            return Forbid(); // Bloqueia caso o usuário não seja admin nem punter
        }
        var rechargesResponse = recharges.Select(RechargeResponseDto.ConvertToDto);

        return Ok(new PagedResponseDto<RechargeResponseDto>
        {
            Items = rechargesResponse,
            TotalCount = totalCount
        });
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(RechargeRequestDto request)
    {
        var recharge = RechargeRequestDto.ConvertToEntity(request);
        var id = await rechargeRepository.AddAsync(recharge);
        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }

    [HttpGet("id/{id}")]
    public async Task<ActionResult<RechargeResponseDto>> GetById(Guid id)
    {
        var recharge = await rechargeRepository.GetByIdAsync(id);
        if (recharge is null)
        {
            return NotFound();
        }
        return Ok(RechargeResponseDto.ConvertToDto(recharge));
    }
    [Authorize(Roles = $"{Roles.Admin},{Roles.Seller}")]
    [HttpPatch("complete")]
    public async Task<ActionResult> UpdateStatusToCompleted(RechargeRequestDto dto)
    {
        var entityId = User.FindFirst("entityid")?.Value;
        var seller = await this.sellerRepository.GetByIdAsync(Guid.Parse(entityId));
        return Ok(await rechargeRepository.UpdateStatusToCompleted(dto.Id, seller));
    }
    [HttpPut]
    public async Task<ActionResult> Update(RechargeRequestDto request)
    {
        var recharge = RechargeRequestDto.ConvertToEntity(request);
        await rechargeRepository.UpdateAsync(recharge);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await rechargeRepository.RemoveByIdAsync(id);
        return Ok();
    }
}