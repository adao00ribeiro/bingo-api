using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers.Shared;


[ApiController]
[Route("api/v{version:apiversion}/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{


}
