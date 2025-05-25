namespace bingo_api.src.DTOs.Response.report;

public record RoundReportItemDto
{
    public Guid RoundId { get; set; }
    public DateTime RoundTime { get; set; }
    public int CardSaleCount { get; set; }
    public int BotSaleCount { get; set; }
    public decimal Collected { get; set; }
    public decimal BotCollected { get; set; }
    public DateTime? Finished { get; set; }
    public int UserWinners { get; set; }
    public int BotWinners { get; set; }
    public decimal UserAwards { get; set; }
    public decimal BotAwards { get; set; }
    public decimal TotalPrizes { get; set; }
    public decimal Comissions { get; set; }
    public decimal NetValue { get; set; }

}
