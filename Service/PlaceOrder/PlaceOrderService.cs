using AppUtility.Helper;
using AutoMapper;
using Data;
using Data.Models;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using PaymentGateWay.PaymentGateway.PayU;
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
        private readonly ILogger<PlaceOrderService> _logger;
        private readonly IMapper _mapper;
        private readonly IAPILogger _apiLogin;
        public PlaceOrderService(IDapperRepository dapper, ILogger<PlaceOrderService> logger, IMapper mapper, IAPILogger apiLogin)
        {
            _apiLogin = apiLogin;
            _mapper = mapper;
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
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<PlaceOrderResponse> PlaceOrder(RequestBase<PlaceOrderReq> request)
        {
            string sp = "proc_Order";
            var res = new PlaceOrderResponse();
            try
            {
                //PlaceOrder 
                var plaeorderRes = await _dapper.GetAsync<PaymentGatewayRequest>(sp, new
                {
                    UserID = request.LoginId,
                    request.Data.AddressID,
                    request.Data.PaymentMode,
                    request.Data.Remark
                }, CommandType.StoredProcedure);
                if (plaeorderRes.StatusCode == ResponseStatus.Success && plaeorderRes.IsPayment)
                {
                    PayUService p = new PayUService(_logger, _dapper, _mapper, _apiLogin);
                    //initiate PaymentGateWay
                    var pgInitiate = await p.GeneratePGRequestForWeb(plaeorderRes);

                    //return pramCreate
                    res.pgResponse = new PaymentGatewayResponse()
                    {
                        TID = "TID" + plaeorderRes.TID,
                        KeyVals = pgInitiate.KeyVals,
                        URL = plaeorderRes.URL
                    };
                    res.StatusCode = pgInitiate.StatusCode;
                    res.ResponseText = pgInitiate.ResponseText;
                }
                else
                {
                    res.StatusCode = plaeorderRes.StatusCode;
                    res.ResponseText = plaeorderRes.ResponseText;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }

    }
}
