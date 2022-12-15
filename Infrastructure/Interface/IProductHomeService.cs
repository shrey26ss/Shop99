using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IProductHomeService
    {
        Task<IResponse<ProductDetails>> GetProductDetails(SearchItem req);
        Task<IResponse<IEnumerable<ProductAttributes>>> GetProductAttrDetails(SearchItem req);
        Task<IResponse<IEnumerable<ProductPictureInfo>>> GetProductPicDetails(SearchItem req);
        Task<IResponse<IEnumerable<AttributeInfo>>> GetProductAttributeInfo(SearchItem req);
    }
}
