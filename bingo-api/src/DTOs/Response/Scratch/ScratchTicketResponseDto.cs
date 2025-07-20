using bingo_api.src.Entities.Scratch;

using bingo_api.src.DTOs.Response.Scratch.Jsonb;

namespace bingo_api.src.DTOs.Response.Scratch;

public record ScratchTicketResponseDto
{
    public Guid Id { get; set; }
    public int Multiplier { get; set; }
    public decimal PrizeWon { get; set; }
    public bool Revealed { get; set; }
    public ScratchTicketAttributesDto? Attributes { get; set; }
    public Guid ScratchGameId { get; set; }
    public ScratchGameResponseDto? ScratchGame { get; set; }

    internal static ScratchTicketResponseDto ConvertToDto(ScratchTicket entity)
    {
        return new ScratchTicketResponseDto
        {
            Id = entity.Id,
            Multiplier = entity.Multiplier,
            PrizeWon = entity.PrizeWon,
            Revealed = entity.Revealed,
            ScratchGameId = entity.ScratchGameId,
            Attributes = entity.Attributes is null
                ? null
                : ScratchTicketAttributesDto.ConvertToDto(entity.Attributes),
            ScratchGame = entity.ScratchGame is null
                ? null
                : ScratchGameResponseDto.ConvertToDto(entity.ScratchGame)
        };
    }
}