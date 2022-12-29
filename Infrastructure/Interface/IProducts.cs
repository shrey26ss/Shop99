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
        Task<IResponse<IEnumerable<Products>>> GetProducts(RequestBase<SearchItem> request);
        Task<IResponse> AddProductVariant(RequestBase<VariantCombination> request);
        Task<IResponse<IEnumerable<ProductVariantAttributeDetails>>> GetProductVarAttrDetails(SearchItem req);
        Task<IResponse> VariantQuantityUpdate(RequestBase<VariantQuantity> request);
    }
}
