using System.Numerics;
using bingo_api.src.Entities.Blockchain;
using bingo_api.src.Interfaces.blockchain;
using bingo_api.src.Providers;
using Nethereum.Hex.HexTypes;
using Nethereum.StandardTokenEIP20;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace bingo_api.src.Services.Blockchain;

public class EvmBlockchainService : IBlockchainService
{
    private readonly IBlockchainProvider _provider;
    private readonly List<TokenAddress> _tokenAddresses;
    private readonly ILogger<EvmBlockchainService>? _logger;

    public string NetworkName { get; }

    public EvmBlockchainService(string networkName, IBlockchainProvider provider, List<TokenAddress> tokenAddresses, ILogger<EvmBlockchainService>? logger = null)
    {

        NetworkName = networkName;
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _tokenAddresses = tokenAddresses ?? throw new ArgumentNullException(nameof(tokenAddresses));
        _logger = logger;
    }

    private Web3 Web3 => _provider.GetClient();

    private TokenAddress GetTokenAddress(string tokenName)
    {
        var tokenAddress = _tokenAddresses.FirstOrDefault(t =>
            t.Token.Name.Equals(tokenName, StringComparison.OrdinalIgnoreCase));

        if (tokenAddress == null)
            throw new InvalidOperationException($"Token {tokenName} não encontrado para a rede {NetworkName}.");

        return tokenAddress;
    }

    private bool IsNative(TokenAddress tokenAddress) =>
       tokenAddress.Token.IsNative;

    // ---------------- NATIVE ----------------
    public async Task<decimal> GetNativeBalanceAsync(string address)
    {
        try
        {
            var balanceWei = await Web3.Eth.GetBalance.SendRequestAsync(address);
            var balance = Web3.Convert.FromWei(balanceWei);
            _logger?.LogDebug("Saldo nativo da rede {NetworkName} para {Address}: {Balance}", NetworkName, address, balance);
            return balance;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Erro ao obter saldo nativo para {Address}", address);
            throw;
        }
    }

    public async Task<string> SendNativeAsync(string fromPrivateKey, string toAddress, decimal amount)
    {
        try
        {
            var account = new Account(fromPrivateKey);
            var web3 = new Web3(account, ((EvmBlockchainProvider)_provider).GetRpcUrl());

            var transaction = await web3.Eth.GetEtherTransferService()
                .TransferEtherAndWaitForReceiptAsync(toAddress, amount);

            _logger?.LogInformation("Transferência nativa bem-sucedida. Hash: {TxHash}", transaction.TransactionHash);
            return transaction.TransactionHash;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Erro ao enviar moeda nativa");
            throw;
        }
    }

    // ---------------- ERC20 ----------------
    public async Task<int> GetTokenDecimalsAsync(string tokenName)
    {
        try
        {
            var tokenAddress = GetTokenAddress(tokenName);
            if (IsNative(tokenAddress)) return 18; // padrão para ETH/BNB

            var tokenService = new StandardTokenService(Web3, tokenAddress.ContractAddress);
            var decimals = await tokenService.DecimalsQueryAsync();
            _logger?.LogDebug("Token {TokenSymbol} tem {Decimals} casas decimais", tokenName, decimals);
            return (int)decimals;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Erro ao obter decimais do token {TokenSymbol}", tokenName);
            throw;
        }
    }

    public async Task<decimal> GetTokenBalanceAsync(string address, string tokenSymbol)
    {
        try
        {
            var tokenAddress = GetTokenAddress(tokenSymbol);
            if (IsNative(tokenAddress))
                return await GetNativeBalanceAsync(address);

            var tokenService = new StandardTokenService(Web3, tokenAddress.ContractAddress);

            var decimals = await tokenService.DecimalsQueryAsync();
            var balance = await tokenService.BalanceOfQueryAsync(address);

            var divisor = BigInteger.Pow(10, (int)decimals);
            var balanceDecimal = (decimal)balance / (decimal)divisor;

            _logger?.LogDebug("Saldo do token {TokenSymbol} para {Address}: {Balance}", tokenSymbol, address, balanceDecimal);
            return balanceDecimal;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Erro ao obter saldo do token {TokenSymbol} para {Address}", tokenSymbol, address);
            throw;
        }
    }

    public async Task<string> SendTokenAsync(string fromPrivateKey, string toAddress, decimal amount, string tokenSymbol)
    {
        try
        {
            var tokenAddress = GetTokenAddress(tokenSymbol);

            if (IsNative(tokenAddress))
                return await SendNativeAsync(fromPrivateKey, toAddress, amount);

            var account = new Account(fromPrivateKey);
            var web3 = new Web3(account, ((EvmBlockchainProvider)_provider).GetRpcUrl());

            var tokenService = new StandardTokenService(web3, tokenAddress.ContractAddress);
            var decimals = await tokenService.DecimalsQueryAsync();
            var amountInSmallestUnit = Web3.Convert.ToWei(amount, (int)decimals);

            _logger?.LogInformation("Enviando {Amount} {TokenSymbol} de {From} para {To}", amount, tokenSymbol, account.Address, toAddress);

            var transferReceipt = await tokenService.TransferRequestAndWaitForReceiptAsync(toAddress, amountInSmallestUnit);

            if (transferReceipt.Status != null && transferReceipt.Status.Value == 1)
            {
                _logger?.LogInformation("Transferência bem-sucedida. Hash: {TxHash}", transferReceipt.TransactionHash);
                return transferReceipt.TransactionHash;
            }

            throw new InvalidOperationException($"Transferência falhou. Status: {transferReceipt.Status}");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Erro ao enviar token {TokenSymbol}", tokenSymbol);
            throw;
        }
    }

    // ---------------- VERIFICAÇÃO ----------------
    public async Task<bool> VerifyTransactionAsync(string txHash, string expectedToAddress, decimal expectedAmount, string tokenName)
    {
        try
        {
            var tokenAddress = GetTokenAddress(tokenName);
            var web3 = Web3;

            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash);
            if (receipt == null || receipt.Status == null || receipt.Status.Value == 0)
            {
                _logger?.LogWarning("Transação {TxHash} falhou ou não encontrada", txHash);
                return false;
            }

            // caso seja nativo
            if (IsNative(tokenAddress))
            {
                var tx = await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(txHash);
                if (tx == null) return false;

                var valueDecimal = Web3.Convert.FromWei(tx.Value.Value);
                Console.WriteLine("opa" + valueDecimal);
                if (string.Equals(tx.To, expectedToAddress, StringComparison.OrdinalIgnoreCase) && valueDecimal >= expectedAmount)
                {
                    _logger?.LogInformation("Transação nativa verificada: valor {ActualValue}, esperado {ExpectedValue}", valueDecimal, expectedAmount);
                    return true;
                }

                _logger?.LogWarning("Transação nativa {TxHash} não corresponde ao esperado", txHash);
                return false;
            }
            else
            {
                var decimals = await GetTokenDecimalsAsync(tokenName);
                var divisor = BigInteger.Pow(10, decimals);

                var transferEventSignature = "0xddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef";

                foreach (var logItem in receipt.Logs)
                {
                    var log = logItem as Nethereum.RPC.Eth.DTOs.FilterLog;
                    if (log == null || !string.Equals(log.Address, tokenAddress.ContractAddress, StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (log.Topics == null || log.Topics.Length < 3)
                        continue;

                    if (!string.Equals(log.Topics[0]?.ToString(), transferEventSignature, StringComparison.OrdinalIgnoreCase))
                        continue;

                    var toAddressTopic = log.Topics[2]?.ToString() ?? "";
                    var toAddr = "0x" + toAddressTopic.Substring(26);

                    if (!string.Equals(toAddr, expectedToAddress, StringComparison.OrdinalIgnoreCase))
                        continue;

                    var value = new HexBigInteger(log.Data).Value;
                    var valueDecimal = (decimal)value / (decimal)divisor;

                    if (valueDecimal >= expectedAmount)
                        _logger?.LogInformation("Transação verificada: valor {ActualValue}, esperado {ExpectedValue}", valueDecimal, expectedAmount);
                    return true;
                }
                _logger?.LogWarning("Evento Transfer não encontrado ou valor insuficiente para {TxHash}", txHash);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Erro ao verificar transação {TxHash}", txHash);
            throw;
        }
    }

    // ---------------- INFO ----------------
    public async Task<(string Name, string Symbol, int Decimals)> GetTokenInfoAsync(string tokenSymbol)
    {
        try
        {
            var tokenAddress = GetTokenAddress(tokenSymbol);

            if (IsNative(tokenAddress))
                return (NetworkName, tokenSymbol.ToUpper(), 18);

            var tokenService = new StandardTokenService(Web3, tokenAddress.ContractAddress);
            var name = await tokenService.NameQueryAsync();
            var symbol = await tokenService.SymbolQueryAsync();
            var decimals = await tokenService.DecimalsQueryAsync();

            return (name, symbol, (int)decimals);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Erro ao obter informações do token {TokenSymbol}", tokenSymbol);
            throw;
        }
    }
}
