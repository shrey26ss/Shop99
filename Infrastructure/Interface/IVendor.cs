using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IVendor
    {
        Task<Response<bool>> ValidateVendor(RequestBase<SearchItem> request);
        Task<Response> AddUpdate(RequestBase<VendorProfile> request);
        Task<Response<VendorProfile>> GetVendorDetails(RequestBase<SearchItem> request);
    }
}
