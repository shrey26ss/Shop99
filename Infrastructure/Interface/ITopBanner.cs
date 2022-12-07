using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface ITopBanner
    {
        Task<IResponse> AddUpdate(RequestBase<TopBanner> request);
        Task<IResponse<IEnumerable<TopBanner>>> GetDetails(RequestBase<SearchItem> req);
        Task<IResponse> Delete(RequestBase<SearchItem> req);
        Task<IResponse<IEnumerable<TopBanner>>> GetOfferBanner(RequestBase<SearchItem> req);
    }
}
