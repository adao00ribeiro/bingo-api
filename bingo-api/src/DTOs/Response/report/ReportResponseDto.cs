namespace bingo_api.src.DTOs.Response.report;

public record ReportResponseDto<TRow, TStats>()
{
    public List<TRow> Rows { get; set; }
    public TStats Stats { get; set; }
    public DateTime StartingOn { get; set; }
    public DateTime EndingOn { get; set; }
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int RowsCount { get; set; }
}
