using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static PaymentGateWay.PaymentGateway.CashFree.Models;
using Infrastructure.Interface;
using Data;
using Entities.Enums;
using Service.Models;
using AppUtility.APIRequest;
using Entities;
using AppUtility.Extensions;

namespace PaymentGateWay.PaymentGateway.CashFree
{
    public class CashFreeService : PaymentGatewayBase
    {
        private readonly IMapper _mapper;
        private readonly IAPILogger _apiLogin;
        private readonly string apiVersion = "2021-05-21";
        private readonly string ContentType = "application/json";
        private readonly Dictionary<string, string> paymentModes = new Dictionary<string, string>(){
            {"CCRD", "cc"},
            {"DCR", "dc"},
            {"PWLT", "PPI wallet"},
            {"NBNK", "nb"},
            {"UPI", "upi"},
            {"37UPI", "upi"},
        };
        public CashFreeService(ILogger logger, IDapperRepository dapper, IMapper mapper, IAPILogger aPILogin) : base(logger, dapper)
        {
            _mapper = mapper;
            _apiLogin = aPILogin;
        }
        public override async Task<Response<PaymentGatewayResponse>> GeneratePGRequestForWebAsync(PaymentGatewayRequest request)
        {
            Response<PaymentGatewayResponse> res = new Response<PaymentGatewayResponse>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString(),
                Result = new PaymentGatewayResponse<CashfreeOrderResponse>
                {
                    Data = new CashfreeOrderResponse()
                }
            };
            string paymentMode = string.Empty;
            if (paymentModes.ContainsKey(request.OPID ?? string.Empty))
            {
                paymentMode = paymentModes[request.OPID];
            }
            var cashfreeRequest = new CashfreeOrderRequest
            {
                order_amount = (double)request.Amount,
                order_currency = "INR",
                order_id = request.TID,
                payment_capture = 1,
                customer_details = new CustomerDetails
                {
                    customer_id = request.UserID.ToString(),
                    customer_email = request.EmailID,
                    customer_phone = request.MobileNo
                },
                order_meta = new OrderMeta
                {
                    payment_methods = paymentMode,
                    return_url = request.Domain + "/CashFreereturn?order_id={order_id}&order_token={order_token}",
                    notify_url = request.Domain + "/CashFreenotify"//"https://roundpay.net/Callback/45"
                },
                order_expiry_time = DateTime.Now.AddMinutes(30).ToString("yyyy-MM-ddTHH:mm:ssZ")
            };
            CashfreeOrderResponse cashfreeResponse = new CashfreeOrderResponse();
            try
            {
                string baseUrl = request.URL;//"https://sandbox.cashfree.com/pg/orders";
                string clientId = request.MerchantID;
                string secretKey = request.MerchantKey;
                var headers = new Dictionary<string, string>
                {
                    {"x-client-id", clientId},
                    {"x-client-secret", secretKey},
                    {"x-api-version", apiVersion}
                };
                string reponse = await AppWebRequest.O.PostJsonDataUsingHWRTLS(baseUrl, cashfreeRequest, headers).ConfigureAwait(false);
                cashfreeResponse = JsonConvert.DeserializeObject<CashfreeOrderResponse>(reponse);
                res.ResponseText = cashfreeResponse.message;
                if (!(cashfreeResponse.code?.ToLower().In("request_failed", "order_meta.return_url_invalid")) ?? true)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = "Transaction intiated";
                    res.Result = new PaymentGatewayResponse<CashfreeOrderResponse>
                    {
                        URL = cashfreeResponse.payment_link,
                        TID = request.TID,  
                        PGType = PaymentGatewayType.CASHFREE,
                        Data = cashfreeResponse,
                        APIResponse = cashfreeResponse,
                    };
                }
            }
            catch (Exception ex)
            {
                string errorMsg = string.Concat(ex.Message, " | request : ", JsonConvert.SerializeObject(cashfreeRequest), " | response : ", JsonConvert.SerializeObject(cashfreeResponse));
                LogError(errorMsg, this.GetType().Name, nameof(this.GeneratePGRequestForWebAsync));
            }
            if (request.IsLoggingTrue)
            {
                _apiLogin.SaveLog(JsonConvert.SerializeObject(cashfreeRequest), JsonConvert.SerializeObject(cashfreeResponse), "Cashfree", request.TID);
            }
            return res;
        }

        public override async Task<Response<StatusCheckResponse>> StatusCheck(StatusCheckRequest request)
        {
            Response<StatusCheckResponse> res = new Response<StatusCheckResponse>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString(),
                Result = new StatusCheckResponse()
            };
            StringBuilder param = new StringBuilder("appId={{appId}}&secretKey={{secretKey}}&orderId={{orderId}}");
            string cashfreeresp = string.Empty;
            string orderId = string.Empty;
            PaymentGatewayModel pgConfig = new PaymentGatewayModel();
            try
            {
                int tid = request.TID;
                pgConfig = await GetConfiguration(request.PGID);
                request = _mapper.Map<StatusCheckRequest>(pgConfig);
                request.TID = tid;
                orderId = $"TID{request.TID.ToString().PadLeft(5, '0')}";//string.Concat("TID", request.TID.ToString());
                param.Replace("{{appId}}", pgConfig.MerchantID);
                param.Replace("{{secretKey}}", pgConfig.MerchantKey);
                param.Replace("{{orderId}}", orderId);
                cashfreeresp = await AppWebRequest.O.CallUsingHttpWebRequest_POSTAsync(pgConfig.StatusCheckURL, param.ToString());
                if (!string.IsNullOrEmpty(cashfreeresp))
                {
                    var APIResponse = JsonConvert.DeserializeObject<CashfreeStatusResponse>(cashfreeresp);
                    res.ResponseText = APIResponse.reason;
                    if (APIResponse != null && (!APIResponse.status?.Equals("ERROR", StringComparison.OrdinalIgnoreCase) ?? false))
                    {
                        APIResponse.txStatus = APIResponse.txStatus == null ? String.Empty : APIResponse.txStatus;
                        res.Result.OrderId = orderId;
                        res.Result.OrderAmount = APIResponse.orderAmount;
                        res.StatusCode = ResponseStatus.Success;
                        res.Result.OrderStatus = !string.IsNullOrEmpty(APIResponse.txStatus) ? GetOrderStatus(APIResponse.txStatus) : APIResponse.orderStatus;
                        res.Result.APIResponse = APIResponse;
                        res.Result.ReferenceId = APIResponse.referenceId ?? string.Empty;
                        res.Result.IsUpdateDb = true;
                    }
                    else
                    {
                        if (APIResponse.reason?.ToLower() == "order id does not exist")
                        {
                            res.StatusCode = ResponseStatus.Success;
                            res.Result.OrderStatus = "Failed";
                            res.Result.OrderId = orderId;
                            res.Result.IsUpdateDb = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = string.Concat(ex.Message, " | request : ", JsonConvert.SerializeObject(param), " | response : ", JsonConvert.SerializeObject(res.Result));
                LogError(errorMsg, this.GetType().Name, nameof(this.StatusCheck));

            }
            if (pgConfig.IsLoggingTrue)
            {
                _apiLogin.SaveLog(string.Concat(pgConfig.StatusCheckURL, "|", param), JsonConvert.SerializeObject(cashfreeresp), "Cashfree-->Statuscheck", orderId);
            }
            return res;
        }

        private string GetOrderStatus(string txnStatus)
        {
            string sts = "Pending";
            switch (txnStatus.ToUpper())
            {
                case "SUCCESS":
                    sts = "SUCCESS";
                    break;
                case "FAILED":
                    sts = "FAILED";
                    break;
                case "FAIL":
                    sts = "FAILED";
                    break;
                case "USER_DROPPED":
                    sts = "FAILED";
                    break;
            }
            return sts;
        }
    }
}