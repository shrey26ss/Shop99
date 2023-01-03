using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("/api/")]
    public class ProductHomeController : Controller
    {
        private readonly IProductHomeService _product;

        public ProductHomeController(IProductHomeService product) => _product = product;
        [Route("ProductHome/GetProductDetails")]
        public async Task<IActionResult> GetProductDetails(SearchItem req) => Ok(await _product.GetProductDetails(req));
        [Route("ProductHome/GetProductAttrDetails")]
        public async Task<IActionResult> GetProductAttrDetails(SearchItem req) => Ok(await _product.GetProductAttrDetails(req));
        [Route("ProductHome/GetProductAttributeInfo")]
        public async Task<IActionResult> GetProductAttributeInfo(SearchItem req) => Ok(await _product.GetProductAttributeInfo(req));
        [Route("ProductHome/GetProductPicDetails")]
        public async Task<IActionResult> GetProductPicDetails(SearchItem req) => Ok(await _product.GetProductPicDetails(req));

        [Route("ProductHome/GetVariantIdByAttributes")]
        public async Task<IActionResult> GetVariantIdByAttributes(VariantIdByAttributesRequest req) => Ok(await _product.GetVariantIdByAttributes(new RequestBase<VariantIdByAttributesRequest> { Data=req }));
        [Route("ProductHome/GetVariantDetailsByAttributes")]
        public async Task<IActionResult> GetVariantDetailsByAttributes(VariantIdByAttributesRequest req) => Ok(await _product.GetVariantdetailsByAttributes(new RequestBase<VariantIdByAttributesRequest> { Data = req }));
        [Route("ProductHome/GetVariantPicture")]
        public async Task<IActionResult> GetVariantPicture(VariantIdByAttributesRequest req) => Ok(await _product.PictureInformation(new RequestBase<VariantIdByAttributesRequest> { Data = req }));
    }
}
