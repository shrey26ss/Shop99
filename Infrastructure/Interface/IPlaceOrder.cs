using Data.Models;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IPlaceOrder
    {
        Task<IResponse<IEnumerable<PaymentMode>>> GetPaymentMode();
        Task<PlaceOrderResponse> PlaceOrder(RequestBase<PlaceOrderReq> request);
    }
}
