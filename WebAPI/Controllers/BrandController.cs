using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class BrandController : ControllerBase
    {
        private readonly IBrands _brand;

        public BrandController(IBrands brand)
        {
            _brand = brand;
        }

        [Route("Brand/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdateBrand(Brands req)
        {
            return Ok(await _brand.AddUpdate(new RequestBase<Brands>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }

        [Route("Brand/GetBrands")]
        public async Task<IActionResult> GetBrands(SearchItem req)
        {
            return Ok(await _brand.GetBrands(new RequestBase<SearchItem>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [Route("Brand/GetBrandDDL")]
        public async Task<IActionResult> GetBrandDDL()
        {
            return Ok(await _brand.GetBrandDDL());
        }
    }
}
