using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Adapter;

public class PushPayAdapter : IPaymentProvider
{
    private readonly HttpClient _httpClient;

    public PushPayAdapter(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Recharge> CreateRechargeAsync(decimal value, Punter punter, PaymentMethod method)
    {
        if (string.IsNullOrEmpty(method.Token))
        {
            throw new Exception("PushPay sem token");
        }

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.pushinpay.com.br/api/pix/cashIn");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", method.Token);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        string jsonPayload = $@"{{ ""value"": {(int)(value * 100)}, ""webhook_url"": """", ""split_rules"": [] }}";

        var content = new StringContent(jsonPayload, null, "application/json");

        request.Content = content;
        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        string result = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var data = JsonSerializer.Deserialize<QrCodeResponse>(result, options);

        return new Recharge(data, punter.Id);
    }


    public async Task ConsultarPix()
    {

        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.pushinpay.com.br/api/transactions/{ID}");
        request.Headers.Add("Authorization", "Bearer");
        request.Headers.Add("Accept", "application/json");
        var content = new StringContent(string.Empty);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        request.Content = content;
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }
}
public class QrCodeResponse
{
    public Guid Id { get; set; }

    [JsonPropertyName("qr_code")]
    public string QrCode { get; set; }
    public string Status { get; set; }

    [JsonPropertyName("value")]
    public decimal Value { get; set; }

    [JsonPropertyName("webhook_url")]
    public string WebhookUrl { get; set; }

    [JsonPropertyName("qr_code_base64")]
    public string QrCodeBase64 { get; set; }
    public object[] SplitRules { get; set; }
    public string EndToEndId { get; set; }
    public string PayerName { get; set; }
    public string PayerNationalRegistration { get; set; }

}