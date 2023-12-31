﻿using AppUtility.Helper;
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
                res.Result = await _dapper.GetAllAsync<PaymentMode>(sp, new { IsCod }, CommandType.Text);
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
            if (request.Data.PaymentMode != PaymentModes.CASH)
            {
                sp = "proc_Initiatepayment";
            }
            var res = new PlaceOrderResponse()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = nameof(ResponseStatus.Failed),
            };
            try
            {
                //var plaeorderRes = new PaymentGatewayRequest();
                var plaeorderRes = await _dapper.GetAsync<PaymentGatewayRequest>(sp, new
                {
                    UserID = request.LoginId,
                    Amount = 0,
                    request.Data.AddressID,
                    request.Data.PaymentMode,
                    request.Data.Remark,
                    request.Data.IsBuyNow,
                    ServiceId = ServiceTypes.Order,
                    TID = 0,
                    IsCallFromTrg = false,
                    request.Data.Coupon
                }, CommandType.StoredProcedure);
                if (plaeorderRes.StatusCode == ResponseStatus.Success && plaeorderRes.IsPayment)
                {
                    plaeorderRes.IsForAPP = request.Data.IsForApp;
                    plaeorderRes.HashString = request.Data.HashString;
                    plaeorderRes.Domain = _irinfo.GetDomain();
                    plaeorderRes.AlternateDomain = string.IsNullOrEmpty(request.Data.AlternateDomain) ? plaeorderRes.Domain : request.Data.AlternateDomain;
                    PayUService p = new PayUService(_logger, _dapper, _mapper, _apiLogin);
                 
                    var pgInitiate = await p.GeneratePGRequestForWeb(plaeorderRes);
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
        public async Task<CouponApplyResponse> ApplyCoupon(CouponApplyRequest req)
        {
            string sp = "dbo.Proc_ApplyCoupon";
            var res = new CouponApplyResponse();
            try
            {
                res = await _dapper.GetAsync<CouponApplyResponse>(sp,
                    new
                    {
                        UserID = req.UserID,
                        Coupons = req.Coupons,
                        IsRemove = req.IsRemove,
                    }, CommandType.StoredProcedure);
                res.StatusCode = res.StatusCode;
                res.ResponseText = res.ResponseText;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
    }
}
