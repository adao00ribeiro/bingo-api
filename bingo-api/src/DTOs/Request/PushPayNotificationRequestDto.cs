using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;


namespace bingo_api.src.DTOs.Request;

public record PushPayNotificationRequestDto
{

    [FromForm(Name = "id")]
    public Guid Id { get; set; }


    [FromForm(Name = "value")]
    public decimal Value { get; set; }


    [FromForm(Name = "status")]
    public string Status { get; set; }


    [FromForm(Name = "end_to_end_id")]
    public string EndToEndId { get; set; }


    [FromForm(Name = "payer_name")]
    public string PayerName { get; set; }


    [FromForm(Name = "payer_national_registration")]
    public string PayerNationalRegistration { get; set; }
}

