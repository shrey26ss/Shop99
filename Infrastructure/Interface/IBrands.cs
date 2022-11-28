using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IBrands
    {
        Task<Response> AddUpdate(RequestBase<Brands> request);
        Task<Response<IEnumerable<Brands>>> GetBrands(RequestBase<SearchItem> request);
        Task<Response> AddUpdateBrandCategoryMapping(RequestBase<BrandCategoryMapping> request);
        Task<Response<IEnumerable<BrandCategoryMapping>>> GetBrandCategoryMapping(RequestBase<SearchItem> request);
        Task<Response<IEnumerable<BrandsDDL>>> GetBrandDDL();
    }
}
