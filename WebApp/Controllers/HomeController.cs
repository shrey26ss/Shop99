using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.ViewModels;
using WebApp.Servcie;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryAPI _category;
        private readonly IProductsAPI _product;
        public HomeController(ICategoryAPI category, IProductsAPI product)
        {
            _category = category;
            _product = product;
        }

        #region Wesbite terms and privacy and refund pages
        [Route("terms")]
        [HttpGet]
        public async Task<IActionResult> TermsAndCondition()
        {

            return View();
        }
        [Route("privacypolicy")]
        [HttpGet]
        public async Task<IActionResult> PrivacyPolicy()
        {

            return View();
        }
        [Route("refund")]
        [HttpGet]
        public async Task<IActionResult> Refund()
        {

            return View();
        }
        #endregion

        #region Index Dashboard

      

        public async Task<IActionResult> Index()
        {
            return View();
        }
        [Route("LoadTopCategory")]
        [HttpPost]
        public async Task<IActionResult> GetTopCategory()
        {
            var res = _category.GetTopCategory().Result;
            return Json(res);
        }
        [Route("LoadTopBanner")]
        [HttpPost]
        public async Task<IActionResult> GetTopBanner()
        {
            var topb = _category.GetTopBanner().Result;
            var topl = _category.GetTopLowerBanners().Result;
            var res = new TopBannerDashBoard()
            {
                topBanner = topb.Result.ToList(),
                topLowBanner = topl.Result.ToList(),
            };
            return PartialView("partial/topbanner", res);
        }

        [Route("ProductSection")]
        [HttpPost]
        public async Task<IActionResult> ProductSection(int id = 0)
        {
            var res = new ProductSection()
            {
                TabID = id
            };
            var req = new ProductRequest()
            {
                Top = 24,
                OrderBy = ""
            };
            if (id == 1)
            {
                res.ProductsData = _category.GetNewProducts(req).Result;

                return PartialView("partial/ProductSection", res);
            }
            else if (id == 2)
            {
                res.ProductsData = _category.GetFeatureProducts(req).Result;
                return PartialView("partial/ProductSection", res);
            }
            else if (id == 3)
            {
                res.ProductsData = _category.GetOnSaleProducts(req).Result;
                return PartialView("partial/ProductSection", res);
            }
            else if (id == 4)
            {
                res.ProductsData = _category.GetBestSeller(req).Result;
                return PartialView("partial/ProductSection", res);
            }

            else
            {
                res.ProductsData = _category.GetNewProducts(req).Result;
                return PartialView("partial/ProductSection", res);
            }
        }


        [Route("HotDeals")]
        [HttpPost]
        public async Task<IActionResult> HotDeals()
        {
            var req = new ProductRequest()
            {
                Top = 24,
                OrderBy = ""
            };

            var res = _category.GetHotDeals(req).Result;

            return PartialView("partial/HotDeals", res.Result);
        }

        [Route("HotDealsNewProduct")]
        [HttpPost]
        public async Task<IActionResult> HotDealsNewProduct()
        {
            var res = new ProductSection()
            {
                TabID = 0
            };
            var req = new ProductRequest()
            {
                Top = 24,
                OrderBy = ""
            };
             res.ProductsData = _category.GetNewProducts(req).Result;
            return PartialView("partial/HotDealsNewProduct", res);
        }
        #endregion

        #region Product Details
        [Route("Home/ProductDetail/{Id}")]
        [Route("ProductDetail/{Id}")]
        public async Task<IActionResult> ProductDetail(int Id)
        {
            var res = await _product.GetProductDetails(Id);
            return View(res.Result ?? new ProductDetails());
        }
        [Route("Home/ProductDetails/{Id}")]
        [Route("ProductDetails/{Id}")]
        public async Task<IActionResult> ProductDetails(int Id)
        {
           // var res = await _product.GetProductDetails(Id);
            return View(Id);
        }
        [HttpPost]
        [Route("ProductAllDetails")]
        public async Task<IActionResult> ProductAllDetails(int Id)
        {
            var res = await _product.GetProductAllDetails(Id);
            return PartialView("partial/_ProductDetails", res.Result ?? new ProductDetails());
        }
        public async Task<IActionResult> ProductAttrDetail(int Id)
        {
            var res = await _product.GetProductAttrDetails(Id);
            return Ok(res.Result);
        }
        public async Task<IActionResult> ProductAttributeInfo(int Id)
        {
            var res = await _product.GetProductAttributeInfo(Id);
            return Ok(res.Result);
        }
        public async Task<IActionResult> GetProductPicDetails(int Id)
        {
            var res = await _product.GetProductPicDetails(Id);
            return Ok(res.Result);
        }

        #endregion

        #region Products by Category
        [Route("category/{id}")]
        [HttpGet]
        public async Task<IActionResult> ProductsByCategory()
        {
            return View();
        }

        [Route("ProductsByCategoryFilter")]
        [HttpPost]
        public async Task<IActionResult> ProductsByCategoryFilter(int cid, string filters)
        {
            var req = new ProductRequest<CategorFilter>()
            {
                Top = 24,
                OrderBy = "",
                MoreFilters = new CategorFilter
                {
                    Attributes = filters,
                    CategoryId = cid,
                }
            };
            var res = _category.GetProductsByCategory(req, @"/api/Home/ByCategoryProduct").Result;
            return PartialView("Partial/_ProductsByCategory", res);
        }

        [Route("categoryfilters")]
        [HttpPost]
        public async Task<IActionResult> GetCategoryFilters(int cid)
        {
            var req = new ProductRequest<int>()
            {
                Top = 24,
                OrderBy = "",
                MoreFilters = cid
            };
            var res = _category.GetCategoryFilters(req).Result;
            return PartialView("Partial/_categoryfilters", res);
        }
        #endregion

        public async Task<IActionResult> Error(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return View("404");
            }
            return View("Error");
        }

        [HttpPost("GetVariantIdByAttributes")]
        public async Task<IActionResult> GetVariantIdByAttributes(VariantIdByAttributesRequest request)
        {
            return Json(await _product.GetVariantIdByAttributes(request));
        }

        #region Products By ProductID
        [Route("Product/{id}")]
        [HttpGet]
        public async Task<IActionResult> ProductsByProductId()
        {
            return View();
        }
        [Route("ProductsByPID")]
        [HttpPost]
        public async Task<IActionResult> ProductsByPID(int cid, string filters)
        {
            var req = new ProductRequest<ProductFilter>()
            {
                Top = 24,
                OrderBy = "",
                MoreFilters = new ProductFilter
                {
                    Attributes = filters,
                    ProductId = cid
                }
            };
            var res = _category.GetProducts(req, @"/api/Home/ByProductId").Result;
            return PartialView("Partial/_ProductsByCategory", res);
        }

        #endregion

        #region Products By BrandID
        [Route("/Brand/{id}")]
        [HttpGet]
        public async Task<IActionResult> ProductsByBrandId()
        {
            return View();
        }
        [Route("ProductsByBID")]
        [HttpPost]
        public async Task<IActionResult> ProductsByBrandID(int cid, string filters)
        {
            var req = new ProductRequest<BrandFilter>()
            {
                Top = 24,
                OrderBy = "",
                MoreFilters = new BrandFilter
                {
                    Attributes = filters,
                    BrandId = cid
                }
            };
            var res = _category.GetProducts(req, @"/api/Home/ByBrandId").Result;
            return PartialView("Partial/_ProductsByCategory", res);
        }
        #endregion
    }
}
