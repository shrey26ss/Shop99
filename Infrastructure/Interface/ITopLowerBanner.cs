using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface ITopLowerBanner
    {
        Task<Response> AddUpdate(RequestBase<TopLowerBanner> request);
        Task<Response<IEnumerable<TopLowerBanner>>> GetDetails(RequestBase<SearchItem> request);
    }
}
