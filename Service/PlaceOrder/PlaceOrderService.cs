using AppUtility.Helper;
using Data;
using Data.Models;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Service.CartWishList
{
    public class PlaceOrderService : IPlaceOrder
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DapperRepository> _logger;
        public PlaceOrderService(IDapperRepository dapper, ILogger<DapperRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }

        public async Task<IResponse<IEnumerable<PaymentMode>>> GetPaymentMode()
        {
            string sp = @"select * from PaymentMode where IsActive=1 order by ID ";
            var res = new Response<IEnumerable<PaymentMode>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<PaymentMode>(sp, null, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
            }
            return res;
        }

    }
}
