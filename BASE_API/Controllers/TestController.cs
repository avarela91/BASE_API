using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BASE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hello World";
        }
    }
}
