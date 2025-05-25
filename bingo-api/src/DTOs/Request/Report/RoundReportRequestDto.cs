using System.ComponentModel.DataAnnotations;

namespace bingo_api.src.DTOs.Request.Report;

public record RoundReportRequestDto : BaseReportRequestDto
{
    [Required(ErrorMessage = "{0} is required.")]
    public List<Guid> SellerIds { get; set; }
}
