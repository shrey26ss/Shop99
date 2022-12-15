using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IOrderDetailsService : IRepository<OrderDetailsRow, OrderDetailsColumn>
    {
        Task<IResponse> ChengeStatusAsync(OrderDetailsRow req);
    }
}
