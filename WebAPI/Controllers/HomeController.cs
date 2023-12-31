﻿using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("/api/Home")]

    public class HomeController : ControllerBase
    {
        private readonly IHomepageService _homepageService;
        private readonly ITopLowerBanner _topLowerBanner;
        private readonly ITopBanner _topBanner;
        private readonly IBrands _brands;
        private readonly IFiltersService _filters;
        private readonly IOrderDetailsService _orderRepo;
        public HomeController(
            IHomepageService homepageService, ITopLowerBanner topLowerBanner, ITopBanner topBanner, IBrands brands, IFiltersService filters, IOrderDetailsService orderRepo)
        {
            _topBanner = topBanner;
            _topLowerBanner = topLowerBanner;
            _homepageService = homepageService;
            _brands = brands;
            _filters = filters;
            _orderRepo = orderRepo;
        }
        [HttpGet(nameof(TopBanners))]
        public async Task<ActionResult> TopBanners() => Ok(await _topBanner.GetDetails(new RequestBase<SearchItem>()));
        [HttpGet(nameof(OfferBanner))]
        public async Task<ActionResult> OfferBanner() => Ok(await _topBanner.GetOfferBanner(new RequestBase<SearchItem>()));
        [HttpGet(nameof(TopLowerBanners))]
        public async Task<IActionResult> TopLowerBanners() => Ok(await _topLowerBanner.GetDetails(new RequestBase<SearchItem>()));
        [HttpPost(nameof(ByCategoryProduct))]
        public async Task<ActionResult> ByCategoryProduct(ProductRequest<CategorFilter> productRequest) => Ok(await _homepageService.GetProductByCategory(productRequest));
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

        [HttpGet(nameof(Test))]
        public async Task<ActionResult> Test()
        {
            var res = await _orderRepo.GetAsync<OrderDetailsRow>(0, x => x.Product == "demo" && x.Qty == 1);
            return Ok(res);
        }
        [HttpPost(nameof(GetAutoSuggetion))]
        public async Task<ActionResult> GetAutoSuggetion(string searchText = "", int Top = 0) => Ok(await _homepageService.GetAutoSuggetion(searchText, Top));
        [HttpPost(nameof(ByProductId))]
        public async Task<ActionResult> ByProductId(ProductRequest<ProductFilter> productRequest) => Ok(await _homepageService.GetProductByPID(productRequest));
        [HttpPost(nameof(ByBrandId))]
        public async Task<ActionResult> ByBrandId(ProductRequest<BrandFilter> brandRequest) => Ok(await _homepageService.GetProductByBrandID(brandRequest));

        [HttpPost(nameof(AddNewsLetter))]
        public async Task<IActionResult> AddNewsLetter(NewsLetter req)
        {
            return Ok(await _homepageService.AddNewsLetter(new RequestBase<NewsLetter>
            {
                Data = req
            }));
        }
        [HttpPost(nameof(GetProductWithCategory))]
        public async Task<IActionResult> GetProductWithCategory()
        {
            var reslist = await _homepageService.ProductWithCategoryList();
            var response = reslist.Category.Select(category => new
              {
                Category = category,
                Products = reslist.Products.Where(product => product.CategoryId == category.CategoryId).Take(10).ToList()
              }).ToList();
            response = response.Where(x => x.Products.Count() > 0).ToList();
            return Ok(response);
        }
    }
}
