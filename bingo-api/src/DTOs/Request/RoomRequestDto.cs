
using System.ComponentModel.DataAnnotations;
using bingo_api.src.Entities;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.DTOs.Request;

public record RoomRequestDto
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O campo Nome não pode exceder 100 caracteres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O campo Id do dono da sala é Obrigatorio")]
    public Guid OwnerId { get; set; }

    public IFormFile? Image { get; set; }
    internal static Room ConvertToEntity(RoomRequestDto dto)
    {
        return new Room(dto.Name, dto.OwnerId);
    }
}
