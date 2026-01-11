
using bingo_api.src.Entities;

namespace bingo_api.src.DTOs.Response;

public record RoomSellerResponseDto
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public RoomResponseDto Room { get; set; }
    public Guid SellerId { get; set; }
    public SellerResponseDto Seller { get; set; }
    public string AssignedBy { get; set; }
   public RoomSellerResponseDto()
    {
    
    }

    public RoomSellerResponseDto(Guid id, Guid roomId, RoomResponseDto room, Guid sellerId, SellerResponseDto seller, string assignedBy)
    {
        Id = id;
        RoomId = roomId;
        Room = room;
        SellerId = sellerId;
        Seller = seller;
        AssignedBy = assignedBy;
    }

    internal static RoomSellerResponseDto ConvertToDto(RoomSeller roomSeller)
    {

        return new RoomSellerResponseDto(
             roomSeller.Id,
             roomSeller.RoomId,
             null,
             roomSeller.OnlineHouseId,
             null,
             roomSeller.AssignedBy
        );
    }

    internal static RoomSellerResponseDto ConvertToDtoToOnlineHouse(RoomSeller r)
    {
        return new RoomSellerResponseDto
        {
          Id=   r.Id,
           RoomId = r.RoomId,
            Room = RoomResponseDto.ConvertToDto(r.Room)
        };
           
    }

}
