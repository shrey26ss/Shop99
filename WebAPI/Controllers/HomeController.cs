using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("Home/")]
    public class HomeController : ControllerBase
    {
        private readonly IHomepageService _homepageService;
        private readonly ITopLowerBanner _topLowerBanner;
        private readonly ITopBanner _topBanner;
        public HomeController(
            IHomepageService homepageService, ITopLowerBanner topLowerBanner, ITopBanner topBanner)
        {
            _topBanner = topBanner;
            _topLowerBanner = topLowerBanner;
            _homepageService = homepageService;            
        }
        [HttpGet(nameof(TopBanners))]
        public async Task<ActionResult> TopBanners() => Ok(await _topBanner.GetDetails());
        [HttpGet(nameof(TopBanners))]
        public async Task<IActionResult> GetDetails() => Ok(await _topLowerBanner.GetDetails());
        [HttpPost(nameof(ByCategoryProduct))]
        public async Task<ActionResult> ByCategoryProduct(ProductRequest<int> productRequest) => Ok(await _homepageService.GetProductByCategory(productRequest));
        [HttpPost(nameof(RandomProduct))]
        public async Task<ActionResult> RandomProduct(ProductRequest productRequest) => Ok(await _homepageService.GetRandomProduct(productRequest));
        [HttpPost(nameof(NewArrivals))]
        public async Task<ActionResult> NewArrivals(ProductRequest<int> productRequest) => Ok(await _homepageService.GetNewArrivals(productRequest));
        [HttpPost(nameof(BestSellerProduct))]
        public async Task<ActionResult> BestSellerProduct(ProductRequest productRequest) => Ok(await _homepageService.GetBestSellerProduct(productRequest));
        [HttpPost(nameof(OnSaleProducts))]
        public async Task<ActionResult> OnSaleProducts(ProductRequest productRequest) => Ok(await _homepageService.GetOnSaleProducts(productRequest));
        [HttpPost(nameof(HotDeals))]
        public async Task<ActionResult> HotDeals(ProductRequest productRequest) => Ok(await _homepageService.GetHotDeals(productRequest));
    }
}
