using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bingo_api.src.DTOs.Response.report;

public record RoundReportStatsDto
{
      public int TotalCount { get; set; }
        public float CollectedSum { get; set; }
        public float BotCollectedSum { get; set; }
        public float UserAwardsSum { get; set; }
        public float BotAwardsSum { get; set; }
        public float TotalPrizesSum { get; set; }
        public float ComissionsSum { get; set; }
        public float NetValueSum { get; set; }
}
