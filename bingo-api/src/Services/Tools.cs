using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace bingo_api.src.Services;

public static class Tools
{
    public static string ConvertToIso8601(string inputDate)
    {
        try
        {
            // Define os formatos esperados de entrada
            string[] formats = { "dd-MM-yyyy", "yyyy-MM-dd" };

            // Tenta fazer o parse da data
            if (DateTime.TryParseExact(inputDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                // Converte para o formato ISO 8601 (UTC)
                return parsedDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            }

            throw new FormatException("A data fornecida não está em um formato válido.");
        }
        catch (Exception ex)
        {
            // Retorna a mensagem de erro caso ocorra algum problema
            return $"Erro: {ex.Message}";
        }
    }
}