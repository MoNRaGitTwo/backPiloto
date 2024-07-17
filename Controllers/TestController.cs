using Microsoft.AspNetCore.Mvc;

namespace DemoPilotoV1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult TestConnection()
        {
            return Ok("¡La conexión a la base de datos fue exitosa desde el backend!");
        }
    }
}

