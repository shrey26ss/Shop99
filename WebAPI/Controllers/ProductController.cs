using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(AuthenticationSchemes = "Bearer", Roles ="1,3,4")]
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
        public async Task<IActionResult> GetProducts(ProductSearchItem req)
        {
            return Ok(await _products.GetProducts(new RequestBase<ProductSearchItem>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [Route("Product/UpdateIsPublishProduct")]
        public async Task<IActionResult> UpdateIsPublishProduct(UpdateIsPublishProduct req)
        {
            return Ok(await _products.UpdateIsPublishProduct(new RequestBase<UpdateIsPublishProduct>
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
        public async Task<IActionResult> GetProductVarAttrDetails(ProductVarAttrDetailsReq req)
        {
            req.UserID = User.GetLoggedInUserId<int>();
            return Ok(await _products.GetProductVarAttrDetails(req, Convert.ToInt32(User.GetLoggedInUserRoles())));
        }
        [Route("Product/UpdateIsPublishVarAttr")]
        public async Task<IActionResult> UpdateIsPublishVarAttr(UpdateIsPublishProduct req)
        {
            return Ok(await _products.UpdateIsPublishVarAttr(new RequestBase<UpdateIsPublishProduct>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [Route("Product/DeleteVariantImage")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteVariantImage(DeleteVariantReq req)
        {
            return Ok(await _products.DeletevariantImage(new RequestBase<DeleteVariantReq>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [Route("Product/ProductRating")]
        public async Task<IActionResult> ProductRating(ProductRatingReq req)
        {
            return Ok(await _products.ProductRating(new RequestBase<ProductRatingReq>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [AllowAnonymous]
        [Route("Product/ProductWiseRating")]
        public async Task<IActionResult> ProductWiseRating(ProductRatingReq req)
        {
            return Ok(await _products.ProductWiseRatings(new RequestBase<ProductRatingReq>
            {
                Data = req
            }));
        }
        [Route("Product/UpdateAdminApprovelStatus")]
        public async Task<IActionResult> UpdateAdminApprovelStatus(UpdateAdminApprovelStatus req)
        {
            if (req.Remark == null)
                req.Remark = string.Empty;
            return Ok(await _products.UpdateAdminApprovelStatus(new RequestBase<UpdateAdminApprovelStatus>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [Route("Product/UpdateProductVariant")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProductVariant(VariantCombination req)
        {
            return Ok(await _products.UpdateProductVariant(new RequestBase<VariantCombination>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
    }
}
