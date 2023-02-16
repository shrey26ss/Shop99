using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IWebsiteinfo
    {
        Task<IResponse> AddUpdate(RequestBase<WebsiteinfoModel> request);
        Task<IResponse<IEnumerable<WebsiteinfoModel>>> GetDetails(RequestBase<SearchItem> req);
    }
}
