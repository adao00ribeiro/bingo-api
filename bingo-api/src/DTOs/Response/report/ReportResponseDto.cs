namespace bingo_api.src.DTOs.Response.report;

public record ReportResponseDto<TRow, TStats>()
{
    public List<TRow> Rows { get; set; } = new();          // sempre inicializada
    public TStats? Stats { get; set; }                     // opcional
    public DateTime? StartingOn { get; set; }             // opcional
    public DateTime? EndingOn { get; set; }               // opcional
    public int? Page { get; set; }                         // opcional
    public int? PerPage { get; set; }                      // opcional
    public int RowsCount { get; set; }                     // obrigatório

    public static implicit operator ReportResponseDto<TRow, TStats>(ReportResponseDto<object, object> v)
    {
        throw new NotImplementedException();
    }

}
