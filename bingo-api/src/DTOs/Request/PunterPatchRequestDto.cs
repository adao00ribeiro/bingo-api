using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Attribute;

namespace bingo_api.src.DTOs.Request;

public record PunterPatchRequestDto
{
    public string? Name { get; set; }

    [CpfValidation(ErrorMessage = "O CPF informado não é válido.")]
    public string? Cpf { get; set; }
    public string? Phone { get; set; }
}
