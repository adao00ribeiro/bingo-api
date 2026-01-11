using Asp.Versioning;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.Interfaces.Services.Bingo;
using Microsoft.AspNetCore.Authorization;

namespace bingo_api.src.Controllers.Bingo;

[Authorize]

[ApiVersion("1.0")]
public class OnlineHouseController(OnlineHouseService onlineHouseService) : ApiControllerBase
{
    private OnlineHouseService _onlineHouseService = onlineHouseService;
}
