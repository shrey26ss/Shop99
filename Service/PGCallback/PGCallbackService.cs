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
    public interface IPGCallback
    {
        Task<ResponsePG> PayUnotify(RequestBase<PayUResponse> request);
        Task<Response> UpadateTransactionStatus(RequestBase<TransactionStatusRequest> request);
        Task<Response> TransactionStatuscheck(RequestBase<TransactionStatusRequest> request);
    }
    public class PGCallbackService : IPGCallback
    {
        private IDapperRepository _dapper;
        private readonly ILogger<PGCallbackService> _logger;
        private readonly IMapper _mapper;
        private readonly IAPILogger _apiLogin;
        public PGCallbackService(IDapperRepository dapper, ILogger<PGCallbackService> logger, IMapper mapper, IAPILogger apiLogin)
        {
            _apiLogin = apiLogin;
            _mapper = mapper;
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<ResponsePG> PayUnotify(RequestBase<PayUResponse> request)
        {
            string sp = "proc_pgCallBackUpdate";
            var res = new ResponsePG();
            try
            {
                PayUService p = new PayUService(_logger, _dapper, _mapper, _apiLogin);
                var req = new StatusCheckRequest()
                {
                    PGID = PaymentGatewayType.PayU,
                    TID = Convert.ToInt32(request.Data.txnid.Replace("TID", ""))
                };
                var statusCheck = await p.StatusCheckPG(req);
                res = await _dapper.GetAsync<ResponsePG>(sp, new
                {
                    Status=statusCheck.Result.OrderStatus,
                    TID=statusCheck.Result.ReferenceId,
                },CommandType.StoredProcedure);
                res.ResponseText = statusCheck.Result.OrderStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }

        public async Task<Response> UpadateTransactionStatus(RequestBase<TransactionStatusRequest> request)
        {
            string sqlQuery = @"Proc_UpdatePaymentStatus";
            var response = await _dapper.GetAsync<Response>(sqlQuery, new { request.Data.TID, request.Data.Status }, CommandType.StoredProcedure);
            return response;
        }
        public async Task<Response> TransactionStatuscheck(RequestBase<TransactionStatusRequest> request)
        {
            var response = new Response();
            var transactiondetails = new PaymentDetails();
            string sqlQuery = @"SELECT * FROM InitiatePayment where TID = @TID";
            transactiondetails = await _dapper.GetAsync<PaymentDetails>(sqlQuery, new { request.Data.TID}, CommandType.Text);
            if (transactiondetails.Status == "P")
            {
                if(transactiondetails.PGID == PaymentGatewayType.PayU)
                {
                    var req = new StatusCheckRequest()
                    {
                        PGID = transactiondetails.PGID,
                        TID = transactiondetails.TID
                    };
                    PayUService p = new PayUService(_logger, _dapper, _mapper, _apiLogin);
                    var statusCheck = await p.StatusCheckPG(req);
                    if (statusCheck.Result.IsUpdateDb)
                    {
                        transactiondetails.Status = statusCheck.Result.OrderStatus.Substring(0, 1);
                        string sp = @"UPDATE InitiatePayment SET [Status] = @Status Where TID = @tid;
                                SELECT 1 StatusCode,'Updated successfully' ResponseText";
                        response = await _dapper.GetAsync<Response>(sp, new { transactiondetails.TID, transactiondetails.Status }, CommandType.Text);
                    }
                }
            }
            return response;
        }
    }
}
