using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace bingo_api.src.DTOs.Request;

public record FilterRoundByRoomIdDateDto
{
    [Required(ErrorMessage = "O ID da sala é obrigatório.")]
    [DefaultValue("9fb5fda5-9c2e-4a81-8e82-7837471ea9c2")]
    public Guid RoomId { get; set; }

    [Required(ErrorMessage = "A data é obrigatória.")]
    [DefaultValue("2024-01-01")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "O horário de início é obrigatório.")]
    [DefaultValue("07:00:00")]
    public TimeSpan StartTime { get; set; }

    [Required(ErrorMessage = "O horário de término é obrigatório.")]
    [DefaultValue("23:59:00")]
    public TimeSpan EndTime { get; set; }

    [Required(ErrorMessage = "O ID do dono é obrigatório.")]
    [DefaultValue("b9c2d2b5-eeae-486c-85ea-06dd5cfe0c06")]
    public Guid PunterId { get; set; }
}
