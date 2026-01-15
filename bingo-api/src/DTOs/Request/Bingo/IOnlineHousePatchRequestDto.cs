using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Structs.OnlineHouse;

namespace bingo_api.src.DTOs.Request.Bingo;

public record IOnlineHousePatchRequestDto
{
     public string? Name { get; set; }
    public string? Hostname { get; set; }
    public OnlineHouseSettings? Settings { get; set; }
}
