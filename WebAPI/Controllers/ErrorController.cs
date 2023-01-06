using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Redirect("~/apidoc");
        }
    }
}
