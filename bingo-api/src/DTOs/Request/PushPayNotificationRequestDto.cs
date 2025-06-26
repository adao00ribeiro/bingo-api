using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace bingo_api.src.DTOs.Request;

public record PushPayNotificationRequestDto
{

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("http_status")]
    public string HttpStatus { get; set; }

    [JsonPropertyName("http_error")]
    public string? HttpError { get; set; }

    [JsonPropertyName("attempt")]
    public int Attempt { get; set; }

    [JsonPropertyName("complete")]
    public bool Complete { get; set; }

    [JsonPropertyName("transaction_id")]
    public string TransactionId { get; set; }

    [JsonPropertyName("transfer_id")]
    public string? TransferId { get; set; }

    [JsonPropertyName("account_id")]
    public string AccountId { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}

