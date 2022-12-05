using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.ViewModels;
using WebApp.Servcie;

namespace WebApp.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<UserHomeController> _logger;
        private readonly ICategoryAPI _category;
        public HomeController(ILogger<UserHomeController> logger, ICategoryAPI category)
        {
            _logger = logger;
            _category = category;
        }
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
        public async Task<IActionResult> Error(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return View("404");
            }
            return View("Error");
        }


    }
}
