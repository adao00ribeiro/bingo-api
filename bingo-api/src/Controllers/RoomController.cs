using System.Security.Claims;
using bingo_api.src.Constants;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace bingo_api.src.Controllers;

[Authorize]
[ApiVersion("1.0")]
public class RoomController(IRoomRepository _roomRepository) : ApiControllerBase
{
    private readonly IRoomRepository roomRepository = _roomRepository;


    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomResponseDto>>> GetAll()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
        {
            return Unauthorized("Usuário não autenticado.");
        }
        IEnumerable<Room> rooms = new List<Room>();
        if (User.IsInRole("Admin"))
        {
            // Admin pode ver todas as salas
            rooms = await roomRepository.GetAllAsync(includeProperties :[r => r.RoomsSellers, r => r.Owner]);
        }
        else if (User.IsInRole("Seller"))
        {
            rooms = await roomRepository.GetAllAsync(filter:r => r.OwnerId == Guid.Parse(userId),includeProperties:[ r => r.RoomsSellers, r => r.Owner]);
        }
        else if (User.IsInRole("Punter"))
        {
            // Punter pode ver apenas as salas dos Sellers associados a ele
            rooms = await roomRepository.GetAllAsync(includeProperties:[
                r => r.RoomsSellers.Any(rs => rs.Seller.Punters.Any(p => p.Id == Guid.Parse(userId))),
                r => r.RoomsSellers,
                r => r.Owner]
            );
        }
        else
        {
            return Forbid();
        }
        //var rooms = await roomRepository.GetAllAsync(  );
        return Ok(rooms.Select(r => RoomResponseDto.ConvertToDto(r)));
    }
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(RoomRequestDto request)
    {
        var id = await roomRepository.AddAsync(RoomRequestDto.ConvertToEntity(request));
        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }
    [HttpGet("id/{id}")]
    public async Task<ActionResult<RoomResponseDto>> GetById(Guid id)
    {
        var room = await roomRepository.GetByIdAsync(id);
        if (room is null)
        {
            return NotFound();
        }

        var roomResponse = RoomResponseDto.ConvertToDto(room);
        return Ok(roomResponse);
    }
    [HttpPut()]
    public async Task<ActionResult> Update(RoomRequestDto request)
    {
        var room = RoomRequestDto.ConvertToEntity(request);
        await roomRepository.UpdateAsync(room);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await roomRepository.RemoveByIdAsync(id);
        return Ok();
    }
}