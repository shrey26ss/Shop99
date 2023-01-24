using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IProducts
    {
        Task<IResponse> AddUpdate(RequestBase<Products> request);
        Task<IResponse<IEnumerable<Products>>> GetProducts(RequestBase<ProductSearchItem> request);
        Task<IResponse> AddProductVariant(RequestBase<VariantCombination> request);
        Task<IResponse<IEnumerable<ProductVariantAttributeDetails>>> GetProductVarAttrDetails(SearchItem req);
        Task<IResponse> VariantQuantityUpdate(RequestBase<VariantQuantity> request);
        Task<IResponse> UpdateIsPublishProduct(RequestBase<UpdateIsPublishProduct> request);
        Task<IResponse> UpdateIsPublishVarAttr(RequestBase<UpdateIsPublishProduct> request);
        Task<IResponse<List<string>>> DeletevariantImage(RequestBase<DeleteVariantReq> request);
        Task<IResponse> ProductRating(RequestBase<ProductRatingReq> request);
    }
}
