using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class BrandController : Controller
    {
        private readonly IBrands _brand;

        public BrandController(IBrands brand)
        {
            _brand = brand;
        }

        [Route("Brand/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdateBrand(RequestBase<Brands> request)
        {
            return Ok(await _brand.AddUpdate(request));
        }

        [Route("Brand/GetBrands")]
        public async Task<IActionResult> GetBrands(RequestBase<SearchItem> request)
        {
            return Ok(await _brand.GetBrands(request));
        }

        [Route("Brand/AddBrandCatMap")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddBrandCategoryMapping(RequestBase<BrandCategoryMapping> request)
        {
            return Ok(await _brand.AddUpdateBrandCategoryMapping(request));
        }

        [Route("Brand/GetBrandCatMap")]
        public async Task<IActionResult> GetBrandCategoryMapping(RequestBase<SearchItem> request)
        {
            return Ok(await _brand.GetBrandCategoryMapping(request));
        }
    }
}
