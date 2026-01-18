using System.Text.Json;
using Asp.Versioning;
using bingo_api.src.Constants;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request.Bingo;
using bingo_api.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace bingo_api.src.Controllers.Bingo;

[Authorize(Roles = $"{Roles.Admin},{Roles.Seller}")]
[ApiVersion("1.0")]
public class PaymentController(IPaymentService paymentService) : ApiControllerBase
{
    private IPaymentService _paymentService = paymentService;


    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] PaymentPatchRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entityId = User.FindFirst("entityid")?.Value;
        if (string.IsNullOrWhiteSpace(entityId))
            return Unauthorized("Identificador de entidade não encontrado.");

        var payment = await _paymentService.GetByIdAsync(id);
        if (payment is null)
            return NotFound("asdasda");
        await this._paymentService.SetActiveCurrentPayment(Guid.Parse(entityId));
        // Converte DTO em dicionário de propriedades para atualização parcial
        var updates = request.GetType()
            .GetProperties()
            .Where(p => p.GetValue(request) != null)
            .ToDictionary(
                prop => prop.Name,
                prop => prop.GetValue(request)
            );
        if (!updates.ContainsKey("Active"))
        {
            updates.Add("Active", true);
        }
        else
        {
            updates["Active"] = true;
        }
        var json = JsonSerializer.Serialize(updates);

        Console.WriteLine("ARROMBADO"+json);
        await _paymentService.UpdatePartialAsync(id, updates);

        return Ok(true);
    }

}
