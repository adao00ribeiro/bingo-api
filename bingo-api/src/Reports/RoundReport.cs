using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Context;
using bingo_api.src.DTOs.Request.Report;
using bingo_api.src.DTOs.Response.report;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace bingo_api.src.Reports;

public class RoundReport
{
    private DataContext context;
    public RoundReport(DataContext context)
    {
        this.context = context;
    }
    internal async Task<ReportResponseDto<RoundReportItemDto, RoundReportStatsDto>> GenerateAsync(RoundReportRequestDto dto)
    {
        var query = BuildBaseQuery(dto);
        query = ApplyFilters(query, dto.Filters);
        query = ApplyOrders(query, dto.Orders);
        var stats = await CalculateStatsAsync(query);
        int totalCount = await query.CountAsync();
        if (dto.PerPage > 0)
        {
            query = query.Skip((dto.Page - 1) * dto.PerPage)
                        .Take(dto.PerPage);
        }
        // Obter resultados
        var roundItems = await query.ToListAsync();

        // Mapear para DTOs
        var items = roundItems.ToList();

        // Montar resposta
        return new ReportResponseDto<RoundReportItemDto, RoundReportStatsDto>
        {
            Rows = items,
            Stats = stats,
            StartingOn = dto.StartingOn,
            EndingOn = dto.EndingOn,
            Page = dto.Page,
            PerPage = dto.PerPage,
            RowsCount = totalCount
        };
    }
    private IQueryable<RoundReportItemDto> BuildBaseQuery(RoundReportRequestDto request)
    {
        var result = context.Rounds
       .Where(br => br.Started >= request.StartingOn && br.Started <= request.EndingOn)
       .Where(br => br.Room.Owner != null && request.SellerIds.Contains(br.Room.Owner.Id))
                     .Select(g => new RoundReportItemDto
                     {
                         RoundId = g.Id,
                         RoundTime = g.Started,
                         CardSaleCount = g.CardSaleCount,
                         Collected = g.CardValue * g.CardSaleCount,
                         BotSaleCount = g.Cards.Count(c => c.Punter.IsBot),
                         BotCollected = g.CardValue * g.Cards.Count(c => c.Punter.IsBot),
                         Finished = g.Finished,
                         UserWinners = g.Prizes.SelectMany(p => p.CardWinners).Count(w => !w.Card.Punter.IsBot),
                         BotWinners = g.Prizes
            .SelectMany(p => p.CardWinners)
            .Count(w => w.Card.Punter.IsBot),
                         UserAwards = g.Prizes
            .SelectMany(p => p.CardWinners)
            .Where(w => !w.Card.Punter.IsBot)
            .Sum(w => (decimal?)w.Value) ?? 0,
                         BotAwards = g.Prizes
            .SelectMany(p => p.CardWinners)
            .Where(w => w.Card.Punter.IsBot)
            .Sum(w => (decimal?)w.Value) ?? 0,
                         TotalPrizes = g.Prizes.Select(p => (decimal?)p.Value).Distinct().Sum() ?? 0,
                         Comissions = 0,
                         NetValue = g.CardSaleCount == 0 ? 0 :
            (g.CardValue * g.CardSaleCount) -
            (g.Prizes
                .SelectMany(p => p.CardWinners)
                .Where(w => !w.Card.Punter.IsBot)
                .Sum(w => (decimal?)w.Value) ?? 0) - 0
                     });
        return result;

    }
    private IQueryable<RoundReportItemDto> ApplyFilters(IQueryable<RoundReportItemDto> query, Dictionary<string, object> filters)
    {
        if (filters == null || !filters.Any())
            return query;

        if (filters.TryGetValue("finished", out var finishedObj) && finishedObj is bool finished && finished)
        {
            query = query.Where(x => x.Finished != null);
        }
        /*
        if (filters.TryGetValue("id", out var idObj) && int.TryParse(idObj.ToString(), out var id))
        {
            query = query.Where(x => x.Id == id);
        }
*/
        return query;
    }
    private IQueryable<RoundReportItemDto> ApplyOrders(IQueryable<RoundReportItemDto> query, List<string> orders)
    {
        if (orders == null || !orders.Any())
            return query;

        /*
        foreach (var order in orders)
        {
            if (order.StartsWith("id:"))
            {
                string direction = order.Substring(3).ToLower();
                query = direction == "desc" ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
            }
        }
*/
        return query;
    }

    private async Task<RoundReportStatsDto> CalculateStatsAsync(IQueryable<RoundReportItemDto> query)
    {
        var stats = await query
            .GroupBy(x => 1) // Agrupar tudo
            .Select(g => new RoundReportStatsDto
            {
                TotalCount = g.Count(),
                CollectedSum = g.Sum(x => x.Collected),
                BotCollectedSum = g.Sum(x => x.BotCollected),
                UserAwardsSum = g.Sum(x => x.UserAwards),
                BotAwardsSum = g.Sum(x => x.BotAwards),
                TotalPrizesSum = g.Sum(x => x.TotalPrizes),
                ComissionsSum = g.Sum(x => x.Comissions),
                NetValueSum = g.Sum(x => x.Collected - x.UserAwards - x.Comissions)
            })
            .FirstOrDefaultAsync();

        return stats ?? new RoundReportStatsDto();
    }

}
