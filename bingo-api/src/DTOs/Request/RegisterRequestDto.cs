using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using bingo_api.src.Attribute;
using bingo_api.src.Entities;
using Microsoft.AspNetCore.Identity;

namespace bingo_api.src.DTOs.Request;

public record RegisterRequestDto: IValidatableObject
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [DefaultValue("Jodo joelso")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DefaultValue("default00admin")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} é inválido")]
    [DefaultValue("jogo@gmail.com")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
    [Phone(ErrorMessage = "O Telefone informado não é válido.")]
    [DefaultValue("44999999999")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "O campo CPF é obrigatório.")]
    [CpfValidation(ErrorMessage = "O CPF informado não é válido.")]
    [DefaultValue("11111111111")]
    // [RegularExpression(@"\d{3}\.\d{3}\.\d{3}-\d{2}", ErrorMessage = "O CPF informado não é válido.")]
    public string Cpf { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(50, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 6)]
    [DefaultValue("Jodo123+")]
    public string Password { get; set; }

    [Compare(nameof(Password), ErrorMessage = "As senhas devem ser iguais")]
    [DefaultValue("Jodo123+")]
    public string PasswordConfirmed { get; set; }

    [Required(ErrorMessage = "O campo Data de Nascimento é obrigatório.")]
    [DataType(DataType.DateTime, ErrorMessage = "A Data de Nascimento não é válida.")]
    public DateTime DateBirth { get; set; }



    public static User ConvertToEntityUser(RegisterRequestDto dto)
    {
        return new User
        {
            UserName = dto.UserName,
            Email = dto.Email,
            EmailConfirmed = true,
            PasswordHash = dto.Password,
            PhoneNumber = dto.Phone,
        };
    }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var idadeMinima = 18;
        var hoje = DateTime.Today;
        var idade = hoje.Year - DateBirth.Year;
        if (DateBirth > hoje.AddYears(-idade)) idade--;

        if (idade < idadeMinima)
        {
            yield return new ValidationResult(
                $"É necessário ter no mínimo {idadeMinima} anos.",
                new[] { nameof(DateBirth) });
        }
    }
}
