using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface ITopBanner
    {
        Task<Response> AddUpdate(RequestBase<TopBanner> request);
        Task<Response<IEnumerable<TopBanner>>> GetDetails(RequestBase<SearchItem> request);
    }
}
