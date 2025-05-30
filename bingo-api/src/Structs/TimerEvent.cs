using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bingo_api.src.Structs;

public class TimerEvent
{
    public string Time { get; set; }
    public string Date { get; set; }
    public long Timestamp { get; set; }

    public TimerEvent()
    {
        var now = DateTime.Now;
        Time = now.ToString("dd/MM/yyyy HH:mm:ss");
        Date = now.ToString("dd/MM/yyyy");
        Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}