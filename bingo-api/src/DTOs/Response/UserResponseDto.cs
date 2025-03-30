using Microsoft.AspNetCore.Identity;
namespace bingo_api.src.DTOs.Response;
public record UserResponseDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
   

   
    public UserResponseDto(Guid id, string email,string phoneNumber )
    {
        Id = id;
     
        Email = email;
        PhoneNumber = phoneNumber;
    }
    internal static UserResponseDto ConvertToDto(IdentityUser user)
    {
       
        return new UserResponseDto(
            Guid.Parse(user.Id),
            user.Email,
            user.PhoneNumber
        );
    }

}
