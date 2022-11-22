using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IVariant
    {
        Task<Response> AddUpdate(RequestBase<Variants> request);
        Task<Response<IEnumerable<Variants>>> GetVariants(RequestBase<SearchItem> request);
    }
}
