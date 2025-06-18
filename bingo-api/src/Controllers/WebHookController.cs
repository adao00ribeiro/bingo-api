using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using Microsoft.AspNetCore.Authorization;
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

    [HttpPost]
    public async Task<IActionResult> Receive()
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();

        _logger.LogInformation("Webhook recebido: {payload}", body);
        // Você pode processar aqui os dados: verificar status, valor, etc.
        // Exemplo: string status = payload.status;

        return Ok();
    }
}
