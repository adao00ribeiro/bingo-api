using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities;

namespace bingo_api.src.Structs;

public class WinningCardsInfo
{
    public Punter Punter { get; set; }
    public Card Card { get; set; }
    public decimal ValueOfEachWinner { get; set; }


}
