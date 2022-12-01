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
        Task<IResponse<IEnumerable<TopBanner>>> GetDetails();
    }
}
