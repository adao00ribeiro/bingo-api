namespace bingo_api.src.DTOs.Response.report;

public record RoundReportStatsDto
{
        public int TotalCount { get; set; }
        public decimal CollectedSum { get; set; }
        public decimal BotCollectedSum { get; set; }
        public decimal UserAwardsSum { get; set; }
        public decimal BotAwardsSum { get; set; }
        public decimal TotalPrizesSum { get; set; }
        public decimal ComissionsSum { get; set; }
        public decimal NetValueSum { get; set; }
}
