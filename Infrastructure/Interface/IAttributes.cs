using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IAttributes
    {
        Task<Response> AddUpdate(RequestBase<Attributes> request);
        Task<Response<IEnumerable<Attributes>>> GetAttributes(RequestBase<SearchItem> request);
    }
}
