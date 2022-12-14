using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface ICategoryBrandMapping
    {
        Task<IResponse> AddUpdate(RequestBase<CategoryBrandMapping> request);
        Task<IResponse<IEnumerable<BrandMapping>>> GetMapped(RequestBase<SearchItem> req);
        Task<IResponse<IEnumerable<BrandMapping>>> GetUnMapped(RequestBase<SearchItem> req);
    }
}
