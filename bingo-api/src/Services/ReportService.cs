using bingo_api.src.Context;
using bingo_api.src.DTOs.Request.Report;
using bingo_api.src.DTOs.Response.report;
using bingo_api.src.Interfaces.Services;
using bingo_api.src.Reports;

namespace bingo_api.src.Services;

public class ReportService(DataContext dataContext) : IReportService
{
    private readonly DataContext _dataContext = dataContext;

    public Task<ReportResponseDto<RoundReportItemDto, RoundReportStatsDto>> GenerateRoundReportAsync(RoundReportRequestDto dto)
    {

        return new RoundReport(_dataContext).GenerateAsync(dto);
    }
}
