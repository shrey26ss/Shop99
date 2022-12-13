using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface ICountry
    {
        Task<IResponse> AddUpdate(Country request);
        Task<IResponse> ChangeStatus(SearchItem request);
        Task<IResponse<IEnumerable<Country>>> GetCategoriesDDL();
    }
}
