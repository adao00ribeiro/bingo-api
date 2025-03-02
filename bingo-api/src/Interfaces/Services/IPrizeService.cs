using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities;

namespace bingo_api.src.Interfaces.Services;

public interface IPrizeService
{
    void Execute(IEnumerable<Card> cards , int row , int col);  // Executa a lógica do prêmio
    void SaveWinners();              // Salva os vencedores no banco de dados
}
