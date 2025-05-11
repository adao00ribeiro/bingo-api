namespace bingo_api.src.DTOs.Response.report;

public record RoundReportItemDto
{
        public DateTime RoundTime { get; set; }
        public int CardSaleCount { get; set; }
        public int BotSaleCount { get; set; }
        public float Collected { get; set; }
        public float BotCollected { get; set; }
        public DateTime? Finished { get; set; }
        public int UserWinners { get; set; }
        public int BotWinners { get; set; }
        public float UserAwards { get; set; }
        public float BotAwards { get; set; }
        public float TotalPrizes { get; set; }
        public float Comissions { get; set; }
        public float NetValue { get; set; }
}
