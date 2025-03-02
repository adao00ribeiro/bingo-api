using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Services.Prizes;

public class PrizePlusShapeService : PrizeBaseService
{
    public PrizePlusShapeService(Prize prize)
        : base(prize) { }
    
    protected override bool CheckWinner(Card card, int rows, int cols)
    {
        var isWinner = CheckPlusShape(card, rows, cols);
        if (isWinner)
        {
            ExecuteTopFiveList(card, rows, cols);
        }
        return isWinner;
    }
    
    private bool CheckPlusShape(Card card, int rows, int cols)
    {
        // Verificar se o cartão tem dimensões ímpares para ter um centro definido
        if (rows % 2 == 0 || cols % 2 == 0)
        {
            return false; // Formato de cruz não é possível em matrizes de dimensão par
        }
        
        int middleRow = rows / 2;
        int middleCol = cols / 2;
        
        // Verificar linha do meio (horizontal da cruz)
        for (int col = 0; col < cols; col++)
        {
            int index = middleRow * cols + col;
            if (card.CardMarkedNumbers[index] != 1)
            {
                return false;
            }
        }
        
        // Verificar coluna do meio (vertical da cruz), excluindo a interseção que já foi verificada
        for (int row = 0; row < rows; row++)
        {
            if (row != middleRow) // Evitar verificar a posição central duas vezes
            {
                int index = row * cols + middleCol;
                if (card.CardMarkedNumbers[index] != 1)
                {
                    return false;
                }
            }
        }
        
        return true;
    }
    
    protected override void ExecuteTopFiveList(Card card, int rows, int cols)
    {
        // Se chegamos aqui, o cartão já pode ter o formato de cruz
        // Vamos coletar os índices que deveriam formar a cruz
        
        int middleRow = rows / 2;
        int middleCol = cols / 2;
        var plusShapeIndices = new HashSet<int>();
        var missingNumbers = new List<int>();
        
        // Adicionar todos os índices da linha do meio
        for (int col = 0; col < cols; col++)
        {
            plusShapeIndices.Add(middleRow * cols + col);
        }
        
        // Adicionar todos os índices da coluna do meio, excluindo a interseção
        for (int row = 0; row < rows; row++)
        {
            if (row != middleRow) // Evitar adicionar a posição central duas vezes
            {
                plusShapeIndices.Add(row * cols + middleCol);
            }
        }
        
        // Verificar quais posições não estão marcadas
        foreach (var index in plusShapeIndices)
        {
            if (card.CardMarkedNumbers[index] != 1)
            {
                missingNumbers.Add(card.Numbers[index]);
            }
        }
        
        int lackOfHits = missingNumbers.Count;
        
        prize.SetTopFive(card, lackOfHits, missingNumbers);
    }
}