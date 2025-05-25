using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;

namespace bingo_api.src.Services.Prizes
{
    public class PrizeOuterEdgeService : PrizeBaseService
    {
        public PrizeOuterEdgeService(Prize prize)
            : base(prize) { }

        protected override bool CheckWinner(Card card, int rows, int cols)
        {
            var isWinner = CheckOuterEdge(card, rows, cols);
            if (isWinner)
            {
                ExecuteTopFiveList(card, rows, cols);
            }
            return isWinner;
        }

        private bool CheckOuterEdge(Card card, int rows, int cols)
        {
            // Verificar a primeira linha (topo)
            for (int col = 0; col < cols; col++)
            {
                int index = col;
                if (card.CardMarkedNumbers[index] != 1)
                {
                    return false;
                }
            }

            // Verificar a última linha (base)
            for (int col = 0; col < cols; col++)
            {
                int index = (rows - 1) * cols + col;
                if (card.CardMarkedNumbers[index] != 1)
                {
                    return false;
                }
            }

            // Verificar a primeira coluna (esquerda) - excluindo os cantos que já verificamos
            for (int row = 1; row < rows - 1; row++)
            {
                int index = row * cols;
                if (card.CardMarkedNumbers[index] != 1)
                {
                    return false;
                }
            }

            // Verificar a última coluna (direita) - excluindo os cantos que já verificamos
            for (int row = 1; row < rows - 1; row++)
            {
                int index = row * cols + (cols - 1);
                if (card.CardMarkedNumbers[index] != 1)
                {
                    return false;
                }
            }

            return true;
        }

        protected override void ExecuteTopFiveList(Card card, int rows, int cols)
        {
            var edgeIndices = new HashSet<int>();
            var missingNumbers = new List<int>();

            // Adicionar índices da primeira e última linha
            for (int col = 0; col < cols; col++)
            {
                edgeIndices.Add(col);                       // Primeira linha
                edgeIndices.Add((rows - 1) * cols + col);   // Última linha
            }

            // Adicionar índices da primeira e última coluna (excluindo os cantos já adicionados)
            for (int row = 1; row < rows - 1; row++)
            {
                edgeIndices.Add(row * cols);                // Primeira coluna
                edgeIndices.Add(row * cols + (cols - 1));   // Última coluna
            }

            // Identificar os números não marcados na borda
            foreach (var index in edgeIndices)
            {
                if (card.CardMarkedNumbers[index] != 1)
                {
                    missingNumbers.Add(card.Numbers[index]);
                }
            }

            var lackOfHits = missingNumbers.Count;

            prize.SetTopFive(card, lackOfHits, missingNumbers);
        }
    }
}