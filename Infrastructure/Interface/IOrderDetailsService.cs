using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IOrderDetailsService : IRepository<OrderDetailsRow, OrderDetailsColumn>
    {
        Task<IResponse> ChengeStatusAsync(OrderDetailsRow req);
        //Task<IResponse<IEnumerable<TColumn>>> GetAsync<TColumn>(Expression<Func<TColumn, bool>> predicate);
        //string GetAsync<OrderDetailsRow>(Expression<Func<OrderDetailsRow, bool>> predicate);
    }
}
