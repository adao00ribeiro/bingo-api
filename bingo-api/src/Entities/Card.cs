using System.ComponentModel.DataAnnotations.Schema;
using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities;

public class Card : Entity
{
    public int[] Numbers { get; set; }
    public int Code { get; set; }
    public Guid RoundId { get; set; }
    public Round? Round { get; set; }
    public Guid PunterId { get; set; }
    public Punter? Punter { get; set; }
    public IEnumerable<CardWinner>? CardWinners { get; set; }

    [NotMapped]
    public int[] CardMarkedNumbers { get; set; }
    public void CheckNumberOnTheCard(int number)
    {
        if (CardMarkedNumbers == null)
        {
            CardMarkedNumbers = new int[Numbers.Length];
        }

        int index = Array.IndexOf(Numbers, number);

        if (index >= 0) // Se o índice for válido (ou seja, o número foi encontrado)
        {
            CardMarkedNumbers[index] = 1;
        }
    }

}
