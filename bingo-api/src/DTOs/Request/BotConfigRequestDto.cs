using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities;

namespace bingo_api.src.DTOs.Request;

public record BotConfigRequestDto
{
  [Required(ErrorMessage = "Enabled is required.")]
  public bool Enabled { get; set; }
  [Required(ErrorMessage = "RoomId is required.")]
  public Guid RoomId { get; set; }
  internal static BotConfig ConvertToEntity(BotConfigRequestDto dto)
  {
    return new BotConfig(dto.Enabled,dto.RoomId);
  }
}
