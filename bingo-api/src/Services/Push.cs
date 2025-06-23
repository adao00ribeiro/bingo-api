using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace bingo_api.src.Services;

public class Push
{
    public async Task<QrCodeResponse> CriarPix(decimal value)
    {
        var client = new HttpClient();

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.pushinpay.com.br/api/pix/cashIn");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "34572|yzqObrfNiQqHMfW9B2VN2BVLLcinz37Uf9Hf2bnV69dd34ad");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        string jsonPayload = $@"{{ ""value"": {(int)(value * 100)}, ""webhook_url"": ""https://webhook.site/08a2f9a4-4070-419c-af84-d53ac6eff3cb"", ""split_rules"": [] }}";
     
        var content = new StringContent(jsonPayload, null, "application/json");

        request.Content = content;
        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        string result = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var data = JsonSerializer.Deserialize<QrCodeResponse>(result, options);
        Console.WriteLine(data.ToString());
        return data;
    }
    public async Task ConsultarPix()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.pushinpay.com.br/api/transactions/{ID}");
        request.Headers.Add("Authorization", "Bearer");
        request.Headers.Add("Accept", "application/json");
        var content = new StringContent(string.Empty);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        request.Content = content;
        var response = await client.SendAsync(request);
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