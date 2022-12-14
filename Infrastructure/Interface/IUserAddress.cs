using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IUserAddress
    {
        Task<IResponse> AddUpdate(RequestBase<UserAddress> request);
        Task<IResponse<IEnumerable<UserAddress>>> GetList(Request request);
    }
}
