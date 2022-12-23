using Entities.Enums;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IUserRepo 
    {
        Task<IResponse<IEnumerable<UserDetails>>> GetUserListByRole(Role id);
        Task<IResponse<IEnumerable<VendorProfileList>>> GetVendorList(VendorProfileRequest request = null);
    }
}
