using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IVendor
    {
        Task<IResponse<bool>> ValidateVendor(RequestBase<SearchItem> request);
        Task<IResponse> AddUpdate(RequestBase<VendorProfile> request);
        Task<IResponse<VendorProfile>> GetVendorDetails(RequestBase<SearchItem> request);
    }
}
