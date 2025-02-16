namespace bingo_api.src.DTOs.Response;

public record RegisterResponseDto
{
    public bool Sucesso { get; private set; }
    public List<string> Erros { get; private set; }

    public RegisterResponseDto() =>
        Erros = new List<string>();

    public RegisterResponseDto(bool sucesso = true) : this() =>
        Sucesso = sucesso;

    public void AdicionarErros(IEnumerable<string> erros) =>
        Erros.AddRange(erros);
}
