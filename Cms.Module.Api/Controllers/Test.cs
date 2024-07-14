using Cms.Module.Api.TestObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Modules;

namespace Cms.Module.Api.Controllers
{
    [Feature("Cms.Module.Api")]
    [Route("api/test")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Api"), AllowAnonymous, IgnoreAntiforgeryToken]
    public class TestController : Controller
    {
        private readonly TestObjectManager testObjectManager;

        public TestController(TestObjectManager testObjectManager)
        {
            this.testObjectManager = testObjectManager;
        }
        [HttpGet("value")]
        public IActionResult Value()
        {
            return Ok(new { id = 1 });
        }

        [HttpGet("create/{name}")]
        public async Task<IActionResult> Create(string name)
        {
            await testObjectManager.SaveTestObject(new TestEntity
            {
                Name = name,
            });
            return Ok(new { id = 1 });
        }

        [HttpGet("get/{name}")]
        public async Task<IActionResult> List(string name)
        {

            return Ok(await testObjectManager.GetTestObjectByName(name));
        }
    }
}
