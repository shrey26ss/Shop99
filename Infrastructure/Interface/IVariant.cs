using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IVariant
    {
        Task<Response> AddUpdate(RequestBase<AttributeInfo> request);
        Task<Response<IEnumerable<AttributeInfo>>> GetVariants(RequestBase<SearchItem> request);
    }
}
