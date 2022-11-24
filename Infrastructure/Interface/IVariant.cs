using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IVariant
    {
        Task<Response> AddUpdate(RequestBase<ProductVariant> request);
        Task<Response<IEnumerable<ProductVariant>>> GetVariants(RequestBase<SearchItem> request);
    }
}
