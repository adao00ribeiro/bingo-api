using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using bingo_api.src.DTOs.Response.report;

namespace bingo_api.src.Controllers;

[Authorize]
[ApiVersion("1.0")]
public class RoomController(IRoomRepository _roomRepository) : ApiControllerBase
{
    private readonly IRoomRepository roomRepository = _roomRepository;


    [HttpGet]
    public async Task<ActionResult<ReportResponseDto<RoomResponseDto, object>>> GetAll(
     int? page = null, int? size = null)
    {
        var entityId = User.FindFirst("entityid")?.Value;

        var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        if (string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(userRole))
        {
            return Unauthorized("Usuário não autenticado.");
        }
        IEnumerable<Room> rooms = new List<Room>();
        if (User.IsInRole("Admin"))
        {
            // Admin pode ver todas as salas
            rooms = await roomRepository.GetAllAsync(pageNumber: page, pageSize: size, includeProperties: q => q.Include(x => x.RoomsSellers).Include(x => x.Owner));
        }
        else if (User.IsInRole("Seller"))
        {
            rooms = await roomRepository.GetAllAsync(filter: r => r.OwnerId == Guid.Parse(entityId), includeProperties: q => q.Include(x => x.RoomsSellers).Include(x => x.Owner));
        }
        else if (User.IsInRole("Punter"))
        {
            // Punter pode ver apenas as salas dos Sellers associados a ele
            rooms = await roomRepository.GetAllAsync(includeProperties: q => q.Where(r => r.RoomsSellers
            .Any(rs => rs.Seller.Punters
            .Any(p => p.Id == Guid.Parse(entityId)
            )
        ))
        .Include(r => r.RoomsSellers)
            .ThenInclude(rs => rs.Seller)
                .ThenInclude(s => s.Punters)
        .Include(r => r.Owner)
            );
        }
        else
        {
            return Forbid();
        }
        var roomDtos = rooms.Select(r => RoomResponseDto.ConvertToDto(r)).ToList();

        // Paginação simples
        var pageNumber = page ?? 1;
        var pageSize = size ?? roomDtos.Count;
        var pagedRows = roomDtos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        var response = new ReportResponseDto<RoomResponseDto, object>
        {
            Rows = pagedRows,
            Stats = null,                  // opcional, você pode criar um objeto de estatísticas se quiser
            StartingOn = null,
            EndingOn = null,
            Page = pageNumber,
            PerPage = pageSize,
            RowsCount = roomDtos.Count
        };

        return Ok(response);

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