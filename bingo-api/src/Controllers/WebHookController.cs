using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers;
[ApiVersion("1.0")]
public class WebHookController : ApiControllerBase
{
    private readonly ILogger<WebHookController> _logger;

    public WebHookController(ILogger<WebHookController> logger)
    {
        _logger = logger;
    }

    [HttpPost("pushpay")]
    public async Task<IActionResult> ReceivePushPayWebhook()
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();
        _logger.LogInformation("Webhook recebido: {payload}", body);
        return Ok();
    }
}
