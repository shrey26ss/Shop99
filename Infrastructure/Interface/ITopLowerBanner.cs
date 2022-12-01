using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface ITopLowerBanner
    {
        Task<IResponse> AddUpdate(RequestBase<TopLowerBanner> request);
        Task<IResponse<IEnumerable<TopLowerBanner>>> GetDetails();
    }
}
