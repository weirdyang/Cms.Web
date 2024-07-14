using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cms.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous, IgnoreAntiforgeryToken]
    public class DemoController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetValue(string id)
        {
            return Ok(new { id });
        }
    }
}
