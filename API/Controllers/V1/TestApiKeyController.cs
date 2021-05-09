using API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.V1
{
    [ApiKeyAuth]
    public class TestApiKeyController : ControllerBase
    {
        [HttpGet("message")]
        public IActionResult GetMessage()
        {
            return Ok("ApiKey works :)");
        }
    }
}
