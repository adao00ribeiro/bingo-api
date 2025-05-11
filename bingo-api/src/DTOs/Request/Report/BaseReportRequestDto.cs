using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bingo_api.src.DTOs.Request;

public abstract record BaseReportRequestDto
{
    [Required(ErrorMessage = "{0} is required.")]
    public DateTime StartingOn { get; set; } = DateTime.Now;
    [Required(ErrorMessage = "{0} is required.")]
    public DateTime EndingOn { get; set; } = DateTime.Now;
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = -1;
    public Dictionary<string, object> Filters { get; set; } = new Dictionary<string, object>();
    public List<string> Orders { get; set; } = new List<string>();
}
