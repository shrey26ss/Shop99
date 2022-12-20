using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using PaymentGateWay.PaymentGateway.CashFree;
using PaymentGateWay.PaymentGateway.MitraUPI;
using static PaymentGateWay.PaymentGateway.CashFree.Models;
using Data;
using Infrastructure.Interface;
using Entities.Enums;
using Service.Models;
using AppUtility.Extensions;

namespace PaymentGateWay.PaymentGateway
{
    public class PaymentGatewayService : PaymentGatewayBase, IPaymentGatewayService
    {
        private readonly ILogger<PaymentGatewayService> _logger;
        private readonly IDapperRepository _dapper;
        private readonly IMapper _mapper;
        private readonly IAPILogger _apiLogin;
        public PaymentGatewayService(ILogger<PaymentGatewayService> logger, IDapperRepository dapper, IMapper mapper, IAPILogger aPILogin) : base(logger, dapper)
        {
            _logger = logger;
            _dapper = dapper;
            _mapper = mapper;
            _apiLogin = aPILogin;
        }

        public IPaymentGatewayService Gateway(PaymentGatewayType gatewayType)
        {
            IPaymentGatewayService service = null;
            switch (gatewayType)
            {
                case PaymentGatewayType.CASHFREE:
                    return new CashFreeService(_logger, _dapper, _mapper, _apiLogin);
                case PaymentGatewayType.MitraUPINEW:
                    return new MitraUPIPG(_logger, _dapper, _mapper, _apiLogin);

            }
            return service;
        }

        public Task<Response<CashFreeResponseForApp>> GeneratePGRequestForAppAsync(PaymentGatewayRequest request)
        {
            throw new System.NotImplementedException();
        }

        public override async Task<Response<PaymentGatewayResponse>> GeneratePGRequestForWebAsync(PaymentGatewayRequest request)
        {
            var obj = Gateway(request.PGID);
            obj.GeneratePGRequestForAppAsync(request);
            var res = new Response<PaymentGatewayResponse>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString(),
                Result = new PaymentGatewayResponse()
            };
            switch (request.PGID)
            {
                case PaymentGatewayType.CASHFREE:
                    using (var cashFree = new CashFreeService(_logger, _dapper,_mapper, _apiLogin))
                    {
                        res = await cashFree.GeneratePGRequestForWebAsync(request);
                    }
                    break;
                case PaymentGatewayType.MitraUPINEW:
                    using (var cashFree = new MitraUPIPG(_logger, _dapper,_mapper, _apiLogin))
                    {
                        res = await cashFree.GeneratePGRequestForWebAsync(request);
                    }
                    break;
            }
            return res;
        }

        public async Task<Response<int>> SaveInitiatePayment(PaymentGatewayRequest request, int packageId)
        {
            Response<int> response = new Response<int>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            try
            {
                string sqlQuery = @"insert into InitiatePayment(PGID,Amount,PackageId,UserId,EntryOn,[Status]) Values (@PGID,@Amount,@PackageId,@UserId,GETDATE(),'P')
                                    Select SCOPE_IDENTITY()";
                int tid = await _dapper.GetAsync<int>(sqlQuery, new
                {
                    request.PGID,
                    request.Amount,
                    packageId,
                    request.UserID
                }, System.Data.CommandType.Text);
                if (tid > 0)
                {
                    response.StatusCode = ResponseStatus.Success;
                    response.ResponseText = ResponseStatus.Success.ToString();
                    response.Result = tid;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, new { className = this.GetType().Name, fn = nameof(SaveInitiatePayment) });
            }
            return response;
        }

        public async Task<Response<StatusCheckResponse>> StatusCheck(StatusCheckRequest request)
        {
            Response<StatusCheckResponse> res = new Response<StatusCheckResponse>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString(),
                Result = new StatusCheckResponse()
            };

            /*
             
             */
            res = await GetTransactionStatus(request.TID);
            if(res.Result.OrderStatus.ToLower().In("p","pending"))
            {
                switch (request.PGID)
                {
                    case PaymentGatewayType.CASHFREE:
                        using (var cashFree = new CashFreeService(_logger, _dapper, _mapper, _apiLogin))
                        {
                            res = await cashFree.StatusCheck(request);
                        }
                        break;
                }
            }            
            return res;
        }


        public async Task<Response<StatusCheckResponse>> GetTransactionStatus(int TID)
        {
            Response<StatusCheckResponse> res = new Response<StatusCheckResponse>
            { 
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString(),
                Result = new StatusCheckResponse()
            };
            res.Result = await _dapper.GetAsync<StatusCheckResponse>("select TID OrderId, Amount OrderAmount, [Status] OrderStatus, UTR ReferenceId from InitiatePayment where TID=@TID", new { TID },System.Data.CommandType.Text);
            if (!String.IsNullOrEmpty(res.Result.OrderStatus))
            {
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = ResponseStatus.Success.ToString();
          
            }
            return res;
        }
    }
}
