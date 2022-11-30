using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class ErrorController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
