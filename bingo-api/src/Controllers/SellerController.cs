
using System.Security.Claims;
using bingo_api.src.Constants;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace bingo_api.src.Controllers;


[Authorize]
[ApiVersion("1.0")]
public class SellerController(ISellerRepository _sellerRepository) : ApiControllerBase
{
    private readonly ISellerRepository sellerRepository = _sellerRepository;

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<SellerResponseDto>>> GetAll()
    {
        var sellers = await sellerRepository.GetAllAsync();
        var sellersResponse = sellers.Select(seller => SellerResponseDto.ConvertToDto(seller));
        return Ok(sellersResponse);
    }

    [HttpGet("id/{id}")]
    public async Task<ActionResult<SellerResponseDto>> GetById(Guid id)
    {
        var user = await sellerRepository.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        var userResponse = SellerResponseDto.ConvertToDto(user);
        return Ok(userResponse);
    }
    [HttpGet("me")]
    public async Task<ActionResult<SellerResponseDto>> GetMe()
    {

        var identity = User.Identity as ClaimsIdentity;
        var userEmail = identity?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized(new { message = "Usuário não autenticado." });
        }

        var seller = await this.sellerRepository.GetByEmailAsync(userEmail);
        if (seller is null)
        {
            return NotFound();
        }
        return Ok(SellerResponseDto.ConvertToDto(seller));
    }
    [HttpGet("email/{email}")]
    public async Task<ActionResult<SellerResponseDto>> GetByEmail(string email)
    {
        var user = await sellerRepository.GetByEmailAsync(email);
        if (user is null)
        {
            return NotFound();
        }

        var userResponse = SellerResponseDto.ConvertToDto(user);
        return Ok(userResponse);
    }

    [HttpPost()]
    public async Task<ActionResult<Guid>> Create(SellerRequestDto userRequest)
    {
        var seller = SellerRequestDto.ConvertToEntity(userRequest);
        var id = await sellerRepository.AddAsync(seller);
        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }

    [HttpPut]
    public async Task<ActionResult> Update(SellerRequestDto userRequest)
    {
        var seller = SellerRequestDto.ConvertToEntity(userRequest);
        await sellerRepository.UpdateAsync(seller);
        return Ok();
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await sellerRepository.RemoveByIdAsync(id);
        return Ok();
    }

}
