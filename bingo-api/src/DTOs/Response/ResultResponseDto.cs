namespace bingo_api.src.DTOs.Response;

public record ResultResponseDto
{
    public bool Sucesso { get; private set; }
    public string? Message { get; set; }
    public List<string> Erros { get; private set; }

    public ResultResponseDto() =>
        Erros = new List<string>();

    public ResultResponseDto(bool sucesso = true) : this() =>
        Sucesso = sucesso;
    public ResultResponseDto(bool sucesso, string message) : this()
    {
        Sucesso = sucesso;
        Message = message;
    }

    public void AdicionarErros(IEnumerable<string> erros) =>
        Erros.AddRange(erros);
}
