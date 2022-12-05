using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface ICategoryAttributeMapping
    {
        Task<IResponse> AddUpdate(RequestBase<CategoryAttrMapping> request);
        Task<IResponse<IEnumerable<AtttributeMapping>>> GetMapped(RequestBase<SearchItem> req);
        Task<IResponse<IEnumerable<AtttributeMapping>>> GetUnMapped(RequestBase<SearchItem> req);
    }
}
