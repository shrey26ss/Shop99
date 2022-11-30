using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IAttributes
    {
        Task<IResponse> AddUpdate(RequestBase<Attributes> request);
        Task<IResponse<IEnumerable<Attributes>>> GetAttributes(RequestBase<SearchItem> request);
        Task<IResponse<IEnumerable<AttributesDDL>>> GetAttributeDDL();
    }
}
