using System.Text.Json.Serialization;


namespace bingo_api.src.DTOs.Request;

public record PushPayNotificationRequestDto
{

    public Guid id { get; set; }


    public decimal value { get; set; }


    public string status { get; set; }


    public string end_to_end_id { get; set; }


    public string payer_name { get; set; }

    public string payer_national_registration { get; set; }
}

