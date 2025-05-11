using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.DTOs.Request.Report;
using bingo_api.src.DTOs.Response.report;

namespace bingo_api.src.Interfaces.Services;

public interface IReportService
{
    Task<ReportResponseDto<RoundReportItemDto,RoundReportStatsDto>> GenerateRoundReportAsync(RoundReportRequestDto dto);
}
