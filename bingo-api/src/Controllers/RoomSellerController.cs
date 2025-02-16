
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
public class RoomSellerController(IRoomSellerRepository _roomSellerRepository) : ApiControllerBase
{
    private readonly IRoomSellerRepository roomSellerRepository = _roomSellerRepository;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomSellerResponseDto>>> GetAll()
    {
        var roomSellers = await roomSellerRepository.GetAllAsync();
        var roomSellersResponse = roomSellers.Select(rs => RoomSellerResponseDto.ConvertToDto(rs));
        return Ok(roomSellersResponse);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(RoomSellerRequestDto request)
    {
        var roomSeller = RoomSellerRequestDto.ConvertToEntity(request);
        var id = await roomSellerRepository.AddAsync(roomSeller);
        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }

    [HttpGet("id/{id}")]
    public async Task<ActionResult<RoomSellerResponseDto>> GetById(Guid id)
    {
        var roomSeller = await roomSellerRepository.GetByIdAsync(id);
        if (roomSeller is null)
        {
            return NotFound();
        }
        return Ok(RoomSellerResponseDto.ConvertToDto(roomSeller));
    }

    [HttpPut]
    public async Task<ActionResult> Update(RoomSellerRequestDto request)
    {
        var roomSeller = RoomSellerRequestDto.ConvertToEntity(request);
        await roomSellerRepository.UpdateAsync(roomSeller);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await roomSellerRepository.RemoveByIdAsync(id);
        return Ok();
    }
}