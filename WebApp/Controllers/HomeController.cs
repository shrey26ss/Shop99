﻿using AppUtility.APIRequest;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Routing;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.Middleware;
using WebApp.Models;
using WebApp.Models.ViewModels;
using WebApp.Servcie;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryAPI _category;
        private readonly IProductsAPI _product;
        private string _apiBaseURL;

        public HomeController(ICategoryAPI category, IProductsAPI product, AppSettings appSettings)
        {
            _category = category;
            _product = product;
            _apiBaseURL = appSettings.WebAPIBaseUrl;
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

        [Route("aboutus")]
        [HttpGet]
        public async Task<IActionResult> Aboutus()
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
                Top = 24
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
                Top = 24
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
                Top = 20
            };
            res.ProductsData = _category.GetNewProducts(req).Result;
            return PartialView("partial/HotDealsNewProduct", res);
        }
        #endregion

        #region Product Details
        [Route("Home/ProductDetails/{Id}")]
        [Route("ProductDetails/{Id}")]
        public async Task<IActionResult> ProductDetails(int Id)
        {
            var model = new SEODetails();
            model.ID = Id;
            int UserID = User?.GetLoggedInUserId<int>() ?? 0;
            var res = await _product.GetProductAllDetails(Id, UserID);
            model.OGTittle = res.Result.ProductTitle;
            model.OGImage = res.Result.Thumbnail;
            return View(model);
        }
        [HttpPost]
        [Route("ProductAllDetails")]
        public async Task<IActionResult> ProductAllDetails(int Id)
        {
            int UserID = User?.GetLoggedInUserId<int>() ?? 0;
            var res = await _product.GetProductAllDetails(Id, UserID);
            if (res.StatusCode == ResponseStatus.Failed)
            {
                return Ok(false);
            }
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

        [HttpGet("/products/{filterby}/{id}")]
        public async Task<IActionResult> ProductsByCategory(int id)
        {
            return View();
        }

        [HttpPost("products/" + nameof(Filtered))]
        public async Task<IActionResult> Filtered(string filterBy, int cid, string filters, SortingOption sortBy, int top = 24, int start = 0, int pricefrom = 0, int priceto = 0) => PartialView("Partial/_ProductsByCategory", await FilteredResponse(filterBy, cid, filters, sortBy, top, start, pricefrom, priceto));
        
        [HttpPost]
        public async Task<IActionResult> CategoryProductPartial(ProductRequest<CategorFilter> productRequest)
        {
            var res = await _category.GetProductsByCategory(productRequest, @"/api/Home/ByCategoryProduct");

            return PartialView("Partial/_ByCategoryProduct",res);
        }


        [Route(nameof(Categoryfilters))]
        [HttpPost]
        public async Task<IActionResult> Categoryfilters(int cid)
        
        {
            var req = new ProductRequest<int>()
            {
                Top = 24,
                MoreFilters = cid
            };
            var res = _category.GetCategoryFilters(req).Result;
            return PartialView("partial/_categoryfilters", res.Result);
        }
        [Route(nameof(RoundedCategory))]
        [HttpPost]
        public async Task<IActionResult> RoundedCategory()
        {
            var res = _category.GetTopCategory().Result;
            return PartialView("Partial/_RoundCategory", res);
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
        [HttpPost]
        public async Task<IActionResult> ProductWithCategory()
        {
            var response = new List<ProductWithCategoryHome>();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Home/GetProductWithCategory", string.Empty);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                try
                {
                    response = JsonConvert.DeserializeObject<List<ProductWithCategoryHome>>(apiResponse.Result);
                }
                catch (Exception e)
                {
                }
            }
            return PartialView(response);
        }

        public IActionResult Test()
        {
            return Ok(Request.Headers.ToList());
        }

        private async Task<IResponse<JDataTableResponse<ProductResponse>>> FilteredResponse(string filterBy, int cid, string filters, SortingOption sortBy, int top, int start, int pricefrom , int priceto)
        {
            filterBy = filterBy ?? string.Empty;

            IResponse<JDataTableResponse<ProductResponse>> res = new Response<JDataTableResponse<ProductResponse>>
            {

            };
            switch (filterBy.ToUpper())
            {
                case "CATEGORY":
                    {
                        var req = new ProductRequest<CategorFilter>()
                        {
                            Top = top,
                            OrderBy = sortBy,
                            UserID = User?.GetLoggedInUserId<int>() ?? 0,
                            MoreFilters = new CategorFilter
                            {
                                Attributes = filters,
                                pricefrom = pricefrom,
                                priceto = priceto,
                                CategoryId = cid,
                            },
                            Start = start
                        };
                        res = await _category.GetProductsByCategory(req, @"/api/Home/ByCategoryProduct");
                        break;
                    }
                case "BRAND":
                    {
                        var req = new ProductRequest<BrandFilter>()
                        {
                            Top = top,
                            OrderBy = sortBy,
                            MoreFilters = new BrandFilter
                            {
                                Attributes = filters,
                                pricefrom = pricefrom,
                                priceto = priceto,
                                Id = cid,
                            },
                            Start = start,
                            CalledFrom = 'B'
                        };
                        res = await _category.GetProductsByCategory(req, @"/api/Home/ByBrandId");
                        break;
                    }
                case "PROD":
                    {
                        var req = new ProductRequest<BrandFilter>()
                        {
                            Top = top,
                            OrderBy = sortBy,
                            MoreFilters = new BrandFilter
                            {
                                Attributes = filters,
                                Id = cid,
                            },
                            Start = start,
                            CalledFrom = 'P'
                        };
                        res = await _category.GetProductsByCategory(req, @"/api/Home/ByBrandId");
                        break;
                    }
            }
            return res;
        }
    }
}
