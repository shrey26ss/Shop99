using AppUtility.Helper;
using AutoMapper;
using Data;
using Data.Models;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Http;
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
        private readonly INotifyService _notify;
        private readonly IAPILogger _apiLogin;
        private readonly IRequestInfo _irinfo;
        public PlaceOrderService(IDapperRepository dapper, ILogger<PlaceOrderService> logger, IMapper mapper, IAPILogger apiLogin, IRequestInfo irinfo, INotifyService notify)
        {
            _apiLogin = apiLogin;
            _mapper = mapper;
            _dapper = dapper;
            _logger = logger;
            _irinfo = irinfo;
            _notify = notify;
        }

        public async Task<IResponse<IEnumerable<PaymentMode>>> GetPaymentMode(bool IsCod)
        {
            string sp = @"select * from PaymentMode(nolock) where IsActive=1 and (@IsCod=1 or ID<>1) order by ID ";
            var res = new Response<IEnumerable<PaymentMode>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<PaymentMode>(sp,new { IsCod }, CommandType.Text);
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
            string sp = string.Empty;
            if (request.Data.PaymentMode == PaymentModes.CASH) {  sp = "proc_Order_Test"; }
            else { sp = "proc_Initiatepayment"; }
            var res = new PlaceOrderResponse()
            { 
            StatusCode=ResponseStatus.Failed,
            ResponseText=nameof(ResponseStatus.Failed),
            };
            try
            {
                //PlaceOrder 
                var plaeorderRes = await _dapper.GetAsync<PaymentGatewayRequest>(sp, new
                {
                    UserID = request.LoginId,
                    request.Data.AddressID,
                    request.Data.PaymentMode,
                    request.Data.IsBuyNow,
                    request.Data.Remark,
                    ServiceId = ServiceTypes.Order
                }, CommandType.StoredProcedure);
                if (plaeorderRes.StatusCode == ResponseStatus.Success && plaeorderRes.IsPayment)
                {
                    //  plaeorderRes.Domain = _irinfo.GetDomain();
                    plaeorderRes.Domain =  _irinfo.GetDomain();//"http://localhost:52923";
                    plaeorderRes.AlternateDomain = string.IsNullOrEmpty(request.Data.AlternateDomain) ? plaeorderRes.Domain : request.Data.AlternateDomain;
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
                    #region send Notification
                    await _notify.SaveSMSEmailWhatsappNotification(new SMSEmailWhatsappNotification() { FormatID = MessageFormat.OrderPlaced, IsEmail = true }, request.LoginId);
                    #endregion
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
