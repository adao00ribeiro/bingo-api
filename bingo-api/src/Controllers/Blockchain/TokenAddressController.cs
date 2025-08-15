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
public class TokenAddressController(ITokenAddressRepository tokenAddressRepository) : ApiControllerBase
{
    private readonly ITokenAddressRepository _tokenAddressRepository = tokenAddressRepository;


    [HttpGet()]
    public async Task<ActionResult<IEnumerable<TokenAddressResponseDto>>> GetAll(int? page = null, int? size = null)
    {

        var entityId = User.FindFirst("entityid")?.Value;
        int totalCount;
        IEnumerable<TokenAddress> tokenAddress;
        totalCount = await _tokenAddressRepository.CountAsync();
        tokenAddress = await _tokenAddressRepository.GetAllAsync(page, size ,includeProperties:q => q.Include(x => x.Network)
          .Include(x => x.Token) );

        var networkResponse = tokenAddress.Select(t => TokenAddressResponseDto.ConvertToDto(t));

        return Ok(new PagedResponseDto<TokenAddressResponseDto>
        {
            Items = networkResponse,
            TotalCount = totalCount
        });
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(TokenAddressRequestDto request)
    {
        var tokenAddress = TokenAddressRequestDto.ConvertToEntity(request);
        var id = await _tokenAddressRepository.AddAsync(tokenAddress);
        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }

    [HttpGet("id/{id}")]
    public async Task<ActionResult<TokenAddressResponseDto>> GetById(Guid id)
    {
        var network = await _tokenAddressRepository.GetByIdAsync(id);
        if (network is null)
        {
            return NotFound();
        }
        var userResponse = TokenAddressResponseDto.ConvertToDto(network);
        return Ok(userResponse);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut]
    public async Task<ActionResult> Update(TokenAddressRequestDto request)
    {
        var tokenAddress = TokenAddressRequestDto.ConvertToEntity(request);
        await _tokenAddressRepository.UpdateAsync(tokenAddress);
        return Ok();
    }
    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _tokenAddressRepository.RemoveByIdAsync(id);
        return Ok();
    }
}
