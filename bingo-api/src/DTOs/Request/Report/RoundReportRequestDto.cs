using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bingo_api.src.DTOs.Request.Report;

public record RoundReportRequestDto: BaseReportRequestDto
{
    [Required(ErrorMessage = "{0} is required.")]
     public List<Guid> SellerIds { get; set; }
}
