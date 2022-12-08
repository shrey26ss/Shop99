using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/api/{controller}")]
    public class HomeController : ControllerBase
    {
        private readonly IHomepageService _homepageService;
        private readonly ITopLowerBanner _topLowerBanner;
        private readonly ITopBanner _topBanner;
        private readonly IBrands _brands;
        private readonly IFiltersService _filters;
        public HomeController(
            IHomepageService homepageService, ITopLowerBanner topLowerBanner, ITopBanner topBanner, IBrands brands,IFiltersService filters)
        {
            _topBanner = topBanner;
            _topLowerBanner = topLowerBanner;
            _homepageService = homepageService;
            _brands = brands;
            _filters = filters;
        }
        [HttpGet(nameof(TopBanners))]
        public async Task<ActionResult> TopBanners() => Ok(await _topBanner.GetDetails(new RequestBase<SearchItem>()));
        [HttpGet(nameof(OfferBanner))]
        public async Task<ActionResult> OfferBanner() => Ok(await _topBanner.GetOfferBanner(new RequestBase<SearchItem>()));
        [HttpGet(nameof(TopLowerBanners))]
        public async Task<IActionResult> TopLowerBanners() => Ok(await _topLowerBanner.GetDetails(new RequestBase<SearchItem>()));
        [HttpPost(nameof(ByCategoryProduct))]
        public async Task<ActionResult> ByCategoryProduct(ProductRequest<int> productRequest) => Ok(await _homepageService.GetProductByCategory(productRequest));
        [HttpPost(nameof(RandomProduct))]
        public async Task<ActionResult> RandomProduct(ProductRequest productRequest) => Ok(await _homepageService.GetRandomProduct(productRequest));

        [HttpPost(nameof(NewArrivals))]
        public async Task<ActionResult> NewArrivals(ProductRequest<int> productRequest) => Ok(await _homepageService.GetNewArrivals(productRequest));

        [HttpPost(nameof(FeatureProducts))]
        public async Task<ActionResult> FeatureProducts(ProductRequest productRequest) => Ok(await _homepageService.GetFeatureProducts(productRequest));
        [HttpPost(nameof(BestSellerProduct))]
        public async Task<ActionResult> BestSellerProduct(ProductRequest productRequest) => Ok(await _homepageService.GetBestSellerProduct(productRequest));
        [HttpPost(nameof(OnSaleProducts))]
        public async Task<ActionResult> OnSaleProducts(ProductRequest productRequest) => Ok(await _homepageService.GetOnSaleProducts(productRequest));

        [HttpPost(nameof(HotDeals))]
        public async Task<ActionResult> HotDeals(ProductRequest productRequest) => Ok(await _homepageService.GetHotDeals(productRequest));
        [HttpPost(nameof(TopBrands))]
        public async Task<ActionResult> TopBrands(ProductRequest productRequest) => Ok(await _brands.GetTopBrands(productRequest.Top));
        [HttpPost(nameof(FiltersData))]
        public async Task<ActionResult> FiltersData(ProductRequest<int> productRequest) => Ok(await _filters.GetFiltersByCategory(productRequest.MoreFilters));
    }
}
