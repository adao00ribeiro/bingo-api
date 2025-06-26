using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.DTOs.Request;

public record PushPayNotificationRequestDto
{

     public Guid Id { get; set; }
    public decimal Value { get; set; }
    public string Status { get; set; }

    [FromQuery(Name = "end_to_end_id")]
    public string EndToEndId { get; set; }

    [FromQuery(Name = "payer_name")]
    public string PayerName { get; set; }

    [FromQuery(Name = "payer_national_registration")]
    public string PayerNationalRegistration { get; set; }
}

