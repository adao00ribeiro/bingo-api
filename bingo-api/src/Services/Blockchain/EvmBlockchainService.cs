using System.Numerics;
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
    private readonly Dictionary<string, string> _tokenContracts;
    private readonly ILogger<EvmBlockchainService>? _logger;

    public string NetworkName { get; }

    public EvmBlockchainService(string networkName, IBlockchainProvider provider, Dictionary<string, string> tokenContracts, ILogger<EvmBlockchainService>? logger = null)
    {
        NetworkName = networkName;
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _tokenContracts = tokenContracts ?? throw new ArgumentNullException(nameof(tokenContracts));
        _logger = logger;
    }

    private Web3 Web3 => _provider.GetClient();

    public async Task<int> GetTokenDecimalsAsync(string tokenSymbol)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(tokenSymbol))
                throw new ArgumentException("Símbolo do token não pode ser nulo ou vazio.", nameof(tokenSymbol));

            if (!_tokenContracts.TryGetValue(tokenSymbol, out var contractAddress))
                throw new InvalidOperationException($"Token {tokenSymbol} não encontrado na configuração.");
            var tokenService = new StandardTokenService(Web3, contractAddress);
            var decimals = await tokenService.DecimalsQueryAsync();
            
            _logger?.LogDebug("Token {TokenSymbol} tem {Decimals} casas decimais", tokenSymbol, decimals);
            return (int)decimals;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Erro ao obter decimais do token {TokenSymbol}", tokenSymbol);
            throw;
        }
    }

  public async Task<decimal> GetTokenBalanceAsync(string address, string tokenSymbol)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Endereço não pode ser nulo ou vazio.", nameof(address));

            if (string.IsNullOrWhiteSpace(tokenSymbol))
                throw new ArgumentException("Símbolo do token não pode ser nulo ou vazio.", nameof(tokenSymbol));

            if (!_tokenContracts.TryGetValue(tokenSymbol, out var contractAddress))
                throw new InvalidOperationException($"Token {tokenSymbol} não encontrado na configuração.");

            var web3 = Web3;
            var tokenService = new StandardTokenService(web3, contractAddress);
            
            var decimals = await tokenService.DecimalsQueryAsync();
            var balance = await tokenService.BalanceOfQueryAsync(address);
            
            // Conversão mais segura
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
            if (string.IsNullOrWhiteSpace(fromPrivateKey))
                throw new ArgumentException("Chave privada não pode ser nula ou vazia.", nameof(fromPrivateKey));

            if (string.IsNullOrWhiteSpace(toAddress))
                throw new ArgumentException("Endereço de destino não pode ser nulo ou vazio.", nameof(toAddress));

            if (amount <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero.", nameof(amount));

            if (!_tokenContracts.TryGetValue(tokenSymbol, out var contractAddress))
                throw new InvalidOperationException($"Token {tokenSymbol} não encontrado na configuração.");

            // Criar conta a partir da chave privada
            var account = new Account(fromPrivateKey);
            var web3 = new Web3(account, ((EvmBlockchainProvider)_provider).GetRpcUrl());
            
            var tokenService = new StandardTokenService(web3, contractAddress);
            
            // Obter decimais do token
            var decimals = await tokenService.DecimalsQueryAsync();
            
            // Converter amount para a unidade base do token (wei-like)
            var amountInSmallestUnit = Web3.Convert.ToWei(amount, (int)decimals);
            
            _logger?.LogInformation("Enviando {Amount} {TokenSymbol} de {From} para {To}", 
                amount, tokenSymbol, account.Address, toAddress);

            // Executar transferência
            var transferReceipt = await tokenService.TransferRequestAndWaitForReceiptAsync(
                toAddress, 
                amountInSmallestUnit
            );

            if (transferReceipt.Status != null && transferReceipt.Status.Value == 1)
            {
                _logger?.LogInformation("Transferência bem-sucedida. Hash: {TxHash}", transferReceipt.TransactionHash);
                return transferReceipt.TransactionHash;
            }
            else
            {
                throw new InvalidOperationException($"Transferência falhou. Status: {transferReceipt.Status}");
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Erro ao enviar token {TokenSymbol}", tokenSymbol);
            throw;
        }
    }

    public async Task<bool> VerifyTransactionAsync(string txHash, string expectedToAddress, decimal expectedAmount, string tokenSymbol)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txHash))
                throw new ArgumentException("Hash da transação não pode ser nulo ou vazio.", nameof(txHash));

            if (string.IsNullOrWhiteSpace(expectedToAddress))
                throw new ArgumentException("Endereço esperado não pode ser nulo ou vazio.", nameof(expectedToAddress));

            if (!_tokenContracts.TryGetValue(tokenSymbol, out var contractAddress))
                throw new InvalidOperationException($"Token {tokenSymbol} não encontrado na configuração.");

            var web3 = Web3;
            
            // Obter o recibo da transação
            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash);
            if (receipt == null)
            {
                _logger?.LogWarning("Recibo da transação {TxHash} não encontrado", txHash);
                return false;
            }

            // Verificar se a transação foi bem-sucedida
            if (receipt.Status == null || receipt.Status.Value == 0)
            {
                _logger?.LogWarning("Transação {TxHash} falhou (status: {Status})", txHash, receipt.Status?.Value);
                return false;
            }

            // Obter decimais do token
            var decimals = await GetTokenDecimalsAsync(tokenSymbol);
            var divisor = BigInteger.Pow(10, decimals);

            // Analisar eventos Transfer nos logs
            var transferEventSignature = "0xddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef"; // Transfer(address,address,uint256)
            
            foreach (var logItem in receipt.Logs)
            {
                var log = logItem as Nethereum.RPC.Eth.DTOs.FilterLog;
                if (log == null) continue;

                // Verificar se é do contrato correto
                if (!string.Equals(log.Address, contractAddress, StringComparison.OrdinalIgnoreCase))
                    continue;

        // Verificar se é um evento Transfer
                if (log.Topics == null || log.Topics.Count() < 3)
                    continue;

                if (!string.Equals(log.Topics[0]?.ToString(), transferEventSignature, StringComparison.OrdinalIgnoreCase))
                    continue;

                // Decodificar endereço 'to' (terceiro tópico)
                var toAddressTopic = log.Topics[2]?.ToString() ?? "";
                var toAddress = "0x" + toAddressTopic.Substring(26); // Remove os primeiros 26 caracteres (0x + 24 zeros)
                
                if (!string.Equals(toAddress, expectedToAddress, StringComparison.OrdinalIgnoreCase))
                    continue;

                // Decodificar valor dos dados
                if (string.IsNullOrEmpty(log.Data?.ToString()) || log.Data?.ToString() == "0x")
                    continue;

                var valueHex = log.Data?.ToString() ?? "0x0";
                var value = new HexBigInteger(valueHex).Value;
                var valueDecimal = (decimal)value / (decimal)divisor;

                _logger?.LogDebug("Evento Transfer encontrado: {Value} para {ToAddress}", valueDecimal, toAddress);

                // Verificar se o valor é pelo menos o esperado
                if (valueDecimal >= expectedAmount)
                {
                    _logger?.LogInformation("Transação verificada com sucesso. Valor: {ActualValue}, Esperado: {ExpectedValue}", 
                        valueDecimal, expectedAmount);
                    return true;
                }
            }

            _logger?.LogWarning("Evento Transfer não encontrado ou valor insuficiente para transação {TxHash}", txHash);
            return false;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Erro ao verificar transação {TxHash}", txHash);
            throw;
        }
    }

    // Método auxiliar para validar endereço Ethereum
    private static bool IsValidEthereumAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return false;

        return address.StartsWith("0x") && address.Length == 42 && 
               address.Substring(2).All(c => char.IsAsciiHexDigit(c));
    }

    // Método para obter informações do token
    public async Task<(string Name, string Symbol, int Decimals)> GetTokenInfoAsync(string tokenSymbol)
    {
        try
        {
            if (!_tokenContracts.TryGetValue(tokenSymbol, out var contractAddress))
                throw new InvalidOperationException($"Token {tokenSymbol} não encontrado na configuração.");

            var web3 = Web3;
            var tokenService = new StandardTokenService(web3, contractAddress);
            
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