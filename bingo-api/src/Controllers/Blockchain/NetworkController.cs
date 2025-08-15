using Asp.Versioning;
using bingo_api.src.Constants;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request.Blockchain;
using bingo_api.src.DTOs.Response;
using bingo_api.src.DTOs.Response.Blockchain;
using bingo_api.src.Entities.Blockchain;
using bingo_api.src.Interfaces.blockchain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Controllers.Blockchain;

[Authorize]

[ApiVersion("1.0")]
public class NetworkController(INetworkRepository networkRepository) : ApiControllerBase
{
    private readonly INetworkRepository _networkRepository = networkRepository;

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<NetworkResponseDto>>> GetAll(int? page = null, int? size = null)
    {

        var entityId = User.FindFirst("entityid")?.Value;
        int totalCount;
        IEnumerable<Network> networks;
        totalCount = await _networkRepository.CountAsync();
        networks = await _networkRepository.GetAllAsync(page, size , includeProperties: q => q.Include(x=>x.TokenAddresses).ThenInclude(t=>t.Token));

        var networkResponse = networks.Select(n => NetworkResponseDto.ConvertToDto(n));

        return Ok(new PagedResponseDto<NetworkResponseDto>
        {
            Items = networkResponse,
            TotalCount = totalCount
        });
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(NetworkRequestDto request)
    {
        var network = NetworkRequestDto.ConvertToEntity(request);
        var id = await _networkRepository.AddAsync(network);
        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }

    [HttpGet("id/{id}")]
    public async Task<ActionResult<NetworkResponseDto>> GetById(Guid id)
    {
        var network = await _networkRepository.GetByIdAsync(id);
        if (network is null)
        {
            return NotFound();
        }
        var userResponse = NetworkResponseDto.ConvertToDto(network);
        return Ok(userResponse);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut]
    public async Task<ActionResult> Update(NetworkRequestDto request)
    {
        var cardWinner = NetworkRequestDto.ConvertToEntity(request);
        await _networkRepository.UpdateAsync(cardWinner);
        return Ok();
    }
    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _networkRepository.RemoveByIdAsync(id);
        return Ok();
    }
}
