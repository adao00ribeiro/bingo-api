using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request.Report;
using bingo_api.src.DTOs.Response.report;
using bingo_api.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers;

[Authorize]

[ApiVersion("1.0")]
public class ReportController(IReportService service) : ApiControllerBase
{
    private readonly IReportService _reportService = service;
    
    [HttpPost("rounds")]
    public async Task<ActionResult<ReportResponseDto<RoundReportItemDto,RoundReportStatsDto>>> Report(RoundReportRequestDto dto)
    {
        return Ok(await _reportService.GenerateRoundReportAsync(dto));
    }
}
