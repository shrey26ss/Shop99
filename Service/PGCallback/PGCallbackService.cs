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
                res = await _dapper.GetAsync<ResponsePG>(sp, statusCheck,CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }

    }
}
