using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities;

namespace bingo_api.src.Structs;

public class TopCardInfo
{
    public TopCardInfo()
    {

    }

    public Card Card { get; set; }
    public Punter Punter { get; set; }
    public List<int> MissingNumbers { get; set; }
    public int Hits { get; set; }
}
