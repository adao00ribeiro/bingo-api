using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using bingo_api.src.Entities;
using bingo_api.src.Enums;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Adapter;

public class PixGoAdapter : IPaymentProvider
{
    private readonly HttpClient _httpClient;

    public PixGoAdapter(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClient.BaseAddress = new Uri("https://pixgo.org/api/v1/");
    }
   public Task<Recharge> CreateRechargeAsync(decimal value, decimal amount, Punter punter, PaymentMethod method, string? network = null, string? Token = null, string? destinationAddress = null, string? txHash = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
   public async Task<PaymentGatewayResult> CreatePaymentAsync(Recharge recharge, Punter punter, PaymentMethod method, CancellationToken cancellationToken)
    {
       var payload = new PixGoCreateChargeRequest
        {
            Amount = recharge.Amount,
            Description = $"Recharge {recharge.Id}",
            ExternalId = recharge.Id.ToString(),
            CustomerName = punter.Name,
            CustomerEmail = punter.Email,
            CustomerCpf = punter.Cpf,
            WebhookUrl = BuildWebhookUrl(punter.OnlineHouse.Hostname)
        };

          using var request = new HttpRequestMessage(HttpMethod.Post, "payment/create");
          
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", method.Token);
            
        request.Content = new StringContent(
            JsonSerializer.Serialize(payload, JsonOptions),
            Encoding.UTF8,
            "application/json");

       var response = await _httpClient.SendAsync(request, cancellationToken);

        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"PixGo error: {body}");

        var result = JsonSerializer.Deserialize<PaymentGatewayResult>(body, JsonOptions)
                     ?? throw new Exception("PixGo resposta inválida.");

        return result;
    }

   

    private static string BuildWebhookUrl(string? hostname)
    {
        if (string.IsNullOrWhiteSpace(hostname))
            throw new ArgumentException("Hostname inválido.");

        if (!hostname.StartsWith("http://") &&
            !hostname.StartsWith("https://"))
        {
            hostname = "https://" + hostname;
        }

        return hostname.TrimEnd('/') + "/webhook/pixgo";
    }

 
    private static readonly JsonSerializerOptions JsonOptions =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition =
                System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
}


public class PixGoCreateChargeRequest
{
    public decimal Amount { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerCpf { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public string? CustomerAddress { get; set; }
    public string? ExternalId { get; set; }
    public string? Description { get; set; }
    public string? WebhookUrl { get; set; }
}

public class PaymentGatewayResult
{
    public string GatewayTransactionId { get; set; } = "";
    public EPaymentStatus Status { get; set; } 
    public string? QrCode { get; set; }
    public string? QrImageUrl { get; set; }
    public DateTime? ExpiresAt { get; set; }

    // Crypto
    public string? WalletAddress { get; set; }
    public string? Network { get; set; }
}