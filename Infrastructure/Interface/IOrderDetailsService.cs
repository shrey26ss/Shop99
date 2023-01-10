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
        Task<IResponse> ChengeStatusAsync(int loginId,OrderDetailsRow req);
        Task<IResponse> UpdateShippingNInvoice(OrderShippedStatus req);
        Task<IResponse<OrderInvoice>> GetInvoiceDetails(int Id);
        Task<IResponse> OrderReplacedConform(OrderReplacedConformReq req);
        //Task<IResponse<IEnumerable<TColumn>>> GetAsync<TColumn>(Expression<Func<TColumn, bool>> predicate);
        //string GetAsync<OrderDetailsRow>(Expression<Func<OrderDetailsRow, bool>> predicate);
    }
}
