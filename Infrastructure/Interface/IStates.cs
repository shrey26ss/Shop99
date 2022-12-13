using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IStates
    {
        Task<IResponse> AddUpdate(States request);
        Task<IResponse> ChangeStatus(SearchItem request);
        Task<IResponse<IEnumerable<States>>> GetList();
        Task<IEnumerable<StateDDL>> GetDDL();
    }
}
