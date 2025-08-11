using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bingo_api.src.Interfaces.blockchain;

public interface IBlockchainService
{
    string NetworkName { get; }
    Task<decimal> GetTokenBalanceAsync(string address, string tokenSymbol);
    Task<string> SendTokenAsync(string fromPrivateKey, string toAddress, decimal amount, string tokenSymbol);
    Task<bool> VerifyTransactionAsync(string txHash, string expectedToAddress, decimal expectedAmount, string tokenSymbol);
    Task<int> GetTokenDecimalsAsync(string tokenSymbol);
}
