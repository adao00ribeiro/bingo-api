using System.ComponentModel.DataAnnotations;

namespace bingo_api.src.DTOs.Request.Report;

public record RoomReportRequestDto : BaseReportRequestDto
{

    [Required(ErrorMessage = "{0} is required.")]
    public string? RoomName { get; set; }
    [Required(ErrorMessage = "{0} is required.")]
    public Guid? SellerId { get; set; }
}
