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
    public class ProductController : ControllerBase
    {
        private readonly IProducts _products;

        public ProductController(IProducts products)
        {
            _products = products;
        }
        [Route("Product/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdateProduct(Products req)
        {
            return Ok(await _products.AddUpdate(new RequestBase<Products>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }

        [Route("Product/GetProducts")]
        public async Task<IActionResult> GetProducts(SearchItem req)
        {
            return Ok(await _products.GetProducts(new RequestBase<SearchItem>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [Route("Product/AddProductVariant")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddProductVariant(VariantCombination req)
        {
            return Ok(await _products.AddProductVariant(new RequestBase<VariantCombination>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [Route("Product/VariantQuantityUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> VariantQuantityUpdate(VariantQuantity req)
        {
            return Ok(await _products.VariantQuantityUpdate(new RequestBase<VariantQuantity>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }

        [Route("Product/GetProductVarAttrDetails")]
        public async Task<IActionResult> GetProductVarAttrDetails(SearchItem req)
        {
            return Ok(await _products.GetProductVarAttrDetails(req));
        }
    }
}
