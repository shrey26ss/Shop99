using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface ICity
    {
        Task<IResponse> AddUpdate(Cities request);
        Task<IResponse> ChangeStatus(SearchItem request);
        Task<IResponse<IEnumerable<Cities>>> GetList();
        Task<IEnumerable<CityDDL>> GetDDL();
    }
}
