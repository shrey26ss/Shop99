using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/api/{controller}")]
    public class HomeController : ControllerBase
    {
        private readonly IHomepageService _homepageService;
        public HomeController(IHomepageService homepageService)
        {
            _homepageService = homepageService;
        }
        [HttpPost(nameof(ByCategoryProduct))]
        public async Task<ActionResult> ByCategoryProduct(ProductRequest<int> productRequest)
        {
            var res = await _homepageService.GetProductByCategory(productRequest);
            return Ok(res);
        }
        [HttpPost(nameof(RandomProduct))]
        public async Task<ActionResult> RandomProduct(ProductRequest productRequest)
        {
            var res = await _homepageService.GetRandomProduct(productRequest);
            return Ok(res);
        }
        [HttpPost(nameof(NewArrivals))]
        public async Task<ActionResult> NewArrivals(ProductRequest<int> productRequest)
        {
            var res = await _homepageService.GetNewArrivals(productRequest);
            return Ok(res);
        }
        [HttpPost(nameof(BestSellerProduct))]
        public async Task<ActionResult> BestSellerProduct(ProductRequest productRequest)
        {
            var res = await _homepageService.GetBestSellerProduct(productRequest);
            return Ok(res);
        }
        [HttpPost(nameof(OnSaleProducts))]
        public async Task<ActionResult> OnSaleProducts(ProductRequest productRequest)
        {
            var res = await _homepageService.GetOnSaleProducts(productRequest);
            return Ok(res);
        }
        [HttpPost(nameof(HotDeals))]
        public async Task<ActionResult> HotDeals(ProductRequest productRequest)
        {
            var res = await _homepageService.GetHotDeals(productRequest);
            return Ok(res);
        }
    }
}
