using System.Text;
using System.Text.Json;


namespace bingo_api.src.Services;

public class TelegamNotifierService(HttpClient httpClient, IConfiguration _configuration)
{

    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration configuration = _configuration;

    public async Task SendMessageAsync(string message)
    {
        var url = $"https://api.telegram.org/bot{configuration["TelegramBot:Token"]}/sendMessage";

        var content = new StringContent(
            JsonSerializer.Serialize(new
            {
                chat_id = configuration["TelegramBot:ChatId"],
                text = message
            }),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            // Log ou tratamento de erro
        }
    }
}
