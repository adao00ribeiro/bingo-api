using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bingo_api.src.DTOs.Response;

public record LoginResponse
{
    public bool Sucesso => Erros.Count == 0;

    public string AccessToken { get; private set; }

    public string RefreshToken { get; private set; }

    public List<string> Erros { get; private set; }

    public LoginResponse() =>
        Erros = new List<string>();

    public LoginResponse(bool sucesso, string accessToken, string refreshToken) : this()
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public void AdicionarErro(string erro) =>
        Erros.Add(erro);

    public void AdicionarErros(IEnumerable<string> erros) =>
        Erros.AddRange(erros);
}
