using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("Home/")]
    public class HomeController : ControllerBase
    {
        private readonly IHomepageService _homepageService;
        public HomeController(IHomepageService homepageService)
        {
            _homepageService = homepageService;
        }
        [HttpPost(nameof(Details))]
        public async Task<ActionResult> Details(ProductRequest<int> productRequest)
        {
            var res = await _homepageService.GetProductByCategory(productRequest);
            return Ok(res);
        }

    }
}
