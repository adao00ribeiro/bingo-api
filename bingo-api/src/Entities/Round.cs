using System.ComponentModel.DataAnnotations.Schema;
using bingo_api.src.Entities.Shared;
using bingo_api.src.Structs;

namespace bingo_api.src.Entities;

public class Round : Entity
{
    public decimal CardValue { get; set; }
    public int[] Numbers { get; set; }
    public int CardSaleCount { get; set; }
    public int TimeBetweenBalls { get; set; }
    public int MaxBalls { get; set; }//utilizado para jogos de 90 ,80,75, 50 ,30
    public int CardRows { get; set; } // Número de linhas na cartela
    public int CardColumns { get; set; } // Número de colunas na cartela
   
    [NotMapped]
    public int CardsPurchased { get; set; }
    public DateTime Started { get; set; }
    public DateTime? Finished { get; set; }
    public Guid RoomId { get; set; }
    public Room? Room { get; set; }
     public List<TimelineEvent> Timeline { get; set; } = new List<TimelineEvent>();
    public IEnumerable<Card>? Cards { get; set; }
    public ICollection<Prize>? Prizes { get; set; }
    public Round()
    {

    }
    public Round(decimal cardValue, DateTime started, int timeBetweenBalls, int maxBalls, int cardRows, int cardColumns, Guid roomId)
    {
        this.CardValue = cardValue;
        this.Numbers = [];
        this.Started = started;
        this.TimeBetweenBalls = timeBetweenBalls;
        this.MaxBalls = maxBalls;
        this.CardRows = cardRows;
        this.CardColumns = cardColumns;
        this.RoomId = roomId;
        this.Prizes = new List<Prize>();
    }
    public void AddPrize(Prize prize)
    {
        if (Prizes == null)
        {
            Prizes = new List<Prize>();
        }
        Prizes.Add(prize); // Adiciona diretamente à lista
    }
}
