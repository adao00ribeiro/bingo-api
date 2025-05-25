using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;

namespace bingo_api.src.Services.Prizes
{
    public class PrizeSingleColumnService : PrizeBaseService
    {
        public PrizeSingleColumnService(Prize prize)
            : base(prize) { }

        protected override bool CheckWinner(Card card, int rows, int cols)
        {
            var isWinner = CheckSingleColumn(card, rows, cols);
            if (isWinner)
            {
                ExecuteTopFiveList(card, rows, cols);
            }
            return isWinner;
        }

        private bool CheckSingleColumn(Card card, int rows, int cols)
        {
            // Verificar cada coluna
            for (int col = 0; col < cols; col++)
            {
                bool isColumnComplete = true;

                // Verificar se todos os números da coluna estão marcados
                for (int row = 0; row < rows; row++)
                {
                    int index = row * cols + col;
                    if (card.CardMarkedNumbers[index] != 1)
                    {
                        isColumnComplete = false;
                        break;
                    }
                }

                if (isColumnComplete)
                {
                    return true; // Encontrou uma coluna completa
                }
            }

            return false; // Nenhuma coluna completa encontrada
        }

        protected override void ExecuteTopFiveList(Card card, int rows, int cols)
        {
            var columnsWithMissingNumbers = new List<(int columnIndex, List<int> missingNumbers, int missingCount)>();

            // Para cada coluna, identificar os números não marcados
            for (int col = 0; col < cols; col++)
            {
                var missingNumbers = new List<int>();

                for (int row = 0; row < rows; row++)
                {
                    int index = row * cols + col;
                    if (card.CardMarkedNumbers[index] != 1)
                    {
                        missingNumbers.Add(card.Numbers[index]);
                    }
                }

                if (missingNumbers.Count > 0)
                {
                    columnsWithMissingNumbers.Add((col, missingNumbers, missingNumbers.Count));
                }
            }

            // Ordenar colunas pelo número de itens faltantes (menor para maior)
            var sortedColumns = columnsWithMissingNumbers.OrderBy(col => col.missingCount).ToList();

            if (sortedColumns.Count > 0)
            {
                // Pegar a coluna que está mais próxima de ser completa
                var closestColumn = sortedColumns.First();
                int lackOfHits = closestColumn.missingCount;

                // Atualizar o top five com os números faltantes desta coluna
                prize.SetTopFive(card, lackOfHits, closestColumn.missingNumbers);
            }
            else
            {
                // Todas as colunas estão completas (o que não deveria acontecer se chegamos aqui)
                prize.SetTopFive(card, 0, new List<int>());
            }
        }
    }
}