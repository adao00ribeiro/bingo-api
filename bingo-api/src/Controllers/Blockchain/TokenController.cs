using Asp.Versioning;
using bingo_api.src.Constants;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request.Blockchain;
using bingo_api.src.DTOs.Response.Blockchain;
using bingo_api.src.DTOs.Response.report;
using bingo_api.src.Entities.Blockchain;
using bingo_api.src.Interfaces.blockchain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers.Blockchain;

[Authorize]
[ApiVersion("1.0")]
public class TokenController(ITokenRepository tokenRepository) : ApiControllerBase
{
    private readonly ITokenRepository _tokenRepository = tokenRepository;

    [HttpGet()]
    public async Task<ActionResult<ReportResponseDto<TokenResponseDto,object>>> GetAll(int? page = null, int? size = null)
    {

        var entityId = User.FindFirst("entityid")?.Value;
        int totalCount;
        IEnumerable<Token> tokens;
        totalCount = await _tokenRepository.CountAsync();
        tokens = await _tokenRepository.GetAllAsync(page, size);

        var tokenResponse = tokens.Select(TokenResponseDto.ConvertToDto).ToList();

          // Paginação simples
        var pageNumber = page ?? 1;
        var pageSize = size ?? tokenResponse.Count;
        var pagedRows = tokenResponse.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        var response = new ReportResponseDto<TokenResponseDto, object>
        {
            Rows = pagedRows,
            Stats = null,                  
            StartingOn = null,
            EndingOn = null,
            Page = pageNumber,
            PerPage = pageSize,
            RowsCount = tokenResponse.Count
        };

        return Ok(response);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(TokenRequestDto request)
    {
        var token = TokenRequestDto.ConvertToEntity(request);
        var id = await _tokenRepository.AddAsync(token);
        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }

    [HttpGet("id/{id}")]
    public async Task<ActionResult<TokenResponseDto>> GetById(Guid id)
    {
        var token = await _tokenRepository.GetByIdAsync(id);
        if (token is null)
        {
            return NotFound();
        }
        var tokenResponse = TokenResponseDto.ConvertToDto(token);
        return Ok(tokenResponse);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut]
    public async Task<ActionResult> Update(TokenRequestDto request)
    {
        var token = TokenRequestDto.ConvertToEntity(request);
        await _tokenRepository.UpdateAsync(token);
        return Ok();
    }
    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _tokenRepository.RemoveByIdAsync(id);
        return Ok();
    }
}
