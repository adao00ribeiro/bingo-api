using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Response.Scratch;
using bingo_api.src.Interfaces.Repositories.Scratch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers.Scratch;

[Authorize]
[ApiVersion("1.0")]
public class ScratchGameController(IScratchGameRepository scratchGameRepository) : ApiControllerBase
{
    private readonly IScratchGameRepository _scratchGameRepository = scratchGameRepository;
    
     [HttpGet]
    public async Task<ActionResult<IEnumerable<ScratchGameResponseDto>>> GetAll()
    {
        var games = await this._scratchGameRepository.GetAllAsync();
        return Ok(games.Select(p => ScratchGameResponseDto.ConvertToDto(p)));
    }
}
