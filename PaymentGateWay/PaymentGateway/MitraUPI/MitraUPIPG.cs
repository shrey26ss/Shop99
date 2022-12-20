using AutoMapper;
using Newtonsoft.Json;
using static PaymentGateWay.PaymentGateway.CashFree.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Infrastructure.Interface;
using Data;
using Service.Models;
using Entities.Enums;
using AppUtility.APIRequest;
using Entities;

namespace PaymentGateWay.PaymentGateway.MitraUPI
{
    public class MitraUPIPG : PaymentGatewayBase
    {

        private static int EXPIRES_IN = 0;
        private static string TOKEN = string.Empty;
        private static string TOKEN_REFRESH = string.Empty;
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
        public MitraUPIPG(ILogger logger, IDapperRepository dapper, IMapper mapper, IAPILogger aPILogin) : base(logger, dapper)
        {
            _mapper = mapper;
            _apiLogin = aPILogin;
        }
        public override async Task<Response<PaymentGatewayResponse>> GeneratePGRequestForWebAsync(PaymentGatewayRequest request)
        {
            string authURL = $"{request.URL}/api/login";
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
            request.URL = request.URL + "api/InitiateTransactionForBuyingPackage";
            var AllUPIRequest = new AllUPIRequest
            {
                requestedId = request.TID.ToString(),//pGTransactionResponse.MerchantID
                amount = request.Amount,
                upiId = request.VPA,
                serverHookURL = request.Domain + "/AllUPInotify",
                webHookURL = request.Domain + "/AllUPIreturn",
                IsSelfCall = true
            };
            bool isRefreshOnly = false, isCallToken = false;
            if (string.IsNullOrEmpty(TOKEN))
            {
                isCallToken = true;
            }
            else if (!string.IsNullOrEmpty(TOKEN) && !string.IsNullOrEmpty(TOKEN_REFRESH) && DateTime.Now.AddHours(-1 * EXPIRES_IN).Hour > 23)
            {
                isCallToken = true;
                isRefreshOnly = true;
            }
            if (isCallToken)
            {
                await TokenGeneration(authURL, request.MerchantID, request.MerchantKey, isRefreshOnly).ConfigureAwait(true);
            }
            try
            {
                int attampt = 0;
            initiateTransaction:
                var headers = new Dictionary<string, string> {
                    {"X-Auth", TOKEN}
                };
                var apiResp = string.Empty;
                if (attampt < 2)
                    apiResp = await AppWebRequest.O.PostJsonDataUsingHWRTLS(request.URL, AllUPIRequest, headers).ConfigureAwait(true);
                if (apiResp?.Contains("Unauthorized") ?? false)
                {
                    await TokenGeneration(authURL, request.MerchantID, request.MerchantKey, isRefreshOnly).ConfigureAwait(true);
                    attampt = attampt + 1;
                    goto initiateTransaction;
                }
                if (apiResp != null)
                {
                    var UGRes = JsonConvert.DeserializeObject<AllUPIInitiateTransactionResponse>(apiResp);
                    if (UGRes != null)
                    {
                        res.ResponseText = UGRes.responseText;
                        if (UGRes.statusCode == 1)
                        {
                            res.StatusCode = ResponseStatus.Failed;
                            res.ResponseText = "Transaction intiated";
                            res.Result.URL = UGRes.url;
                            //res.IntentString = UGRes.intentString;
                            //res.IsIntentAllowed = request.IsIntentAllowed;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                string errorMsg = string.Concat(ex.Message, " | request : ", JsonConvert.SerializeObject(AllUPIRequest), " | response : ", JsonConvert.SerializeObject(request));
                LogError(errorMsg, this.GetType().Name, nameof(this.GeneratePGRequestForWebAsync));
            }
            if (request.IsLoggingTrue)
            {
                _apiLogin.SaveLog(JsonConvert.SerializeObject(AllUPIRequest), JsonConvert.SerializeObject(AllUPIRequest), "MitraUPI", request.TID);
            }
            return res;
        }
        public override async Task<Response<StatusCheckResponse>> StatusCheck(StatusCheckRequest transactionPGLog)
        {
            string TID = $"TID{transactionPGLog.TID.ToString().PadLeft(5, '0')}";
            Response<StatusCheckResponse> payresp = new Response<StatusCheckResponse>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString(),
                Result = new StatusCheckResponse()
            };
            StringBuilder param = new StringBuilder("appId={{appId}}&secretKey={{secretKey}}&orderId={{orderId}}");
            string orderId = string.Empty;
            PaymentGatewayModel pgConfig = new PaymentGatewayModel();
            pgConfig = await GetConfiguration(transactionPGLog.PGID);
            bool isRefreshOnly = false, isCallToken = false;
            if (string.IsNullOrEmpty(TOKEN))
            {
                isCallToken = true;
            }
            else if (!string.IsNullOrEmpty(TOKEN) && !string.IsNullOrEmpty(TOKEN_REFRESH) && DateTime.Now.AddHours(-1 * EXPIRES_IN).Hour > 23)
            {
                isCallToken = true;
                isRefreshOnly = true;
            }
            if (isCallToken)
            {
                await TokenGeneration(pgConfig.StatusCheckURL, pgConfig.MerchantID, pgConfig.MerchantKey, isRefreshOnly).ConfigureAwait(true);
            }

            string apiResp = string.Empty;
            try
            {
                pgConfig.StatusCheckURL = $"{pgConfig.StatusCheckURL}api/status";
                int attampt = 0;
            statusCheckCall:
                var headers = new Dictionary<string, string> {
                {"X-Auth", TOKEN}
            };
                if (attampt < 2)
                    apiResp = await AppWebRequest.O.PostJsonDataUsingHWRTLS($"{pgConfig.StatusCheckURL}?RequestedId={TID}", new { }, headers).ConfigureAwait(true);
                /* Manage Unauth*/
                if (apiResp?.Contains("Unauthorized") ?? false)
                {
                    await TokenGeneration(pgConfig.StatusCheckURL, pgConfig.MerchantID, pgConfig.MerchantKey, isRefreshOnly).ConfigureAwait(true);
                    attampt = attampt + 1;
                    apiResp = "";
                    goto statusCheckCall;
                }
                /* End */
                if (!string.IsNullOrEmpty(apiResp))
                {
                    var APIResponse = JsonConvert.DeserializeObject<AllUPIResponse<AllUPIStatusResponse>>(apiResp);
                    if (APIResponse != null && (!APIResponse.result.status?.Equals("ERROR", StringComparison.OrdinalIgnoreCase) ?? false))
                    {
                        APIResponse.result.status = APIResponse.result.status == null ? String.Empty : APIResponse.result.status;
                        payresp.Result.OrderId = orderId;
                        payresp.Result.OrderAmount = APIResponse.result.amount;
                        payresp.StatusCode = ResponseStatus.Success;
                        payresp.Result.OrderStatus = APIResponse.result.status;
                        payresp.Result.APIResponse = APIResponse;
                        payresp.Result.ReferenceId = APIResponse.result.requestedId ?? string.Empty;
                        payresp.Result.IsUpdateDb = true;
                    }
                    else
                    {
                        if (APIResponse.responseText?.ToLower() == "order id does not exist")
                        {
                            payresp.StatusCode = ResponseStatus.Success;
                            payresp.Result.OrderStatus = "Failed";
                            payresp.Result.OrderId = orderId;
                            payresp.Result.IsUpdateDb = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = string.Concat(ex.Message, " | request : ", JsonConvert.SerializeObject(param), " | response : ", JsonConvert.SerializeObject(payresp.Result));
                LogError(errorMsg, this.GetType().Name, nameof(this.StatusCheck));

            }
            if (pgConfig.IsLoggingTrue)
            {
                _apiLogin.SaveLog(string.Concat(pgConfig.StatusCheckURL, "|", param), JsonConvert.SerializeObject(apiResp), "Cashfree-->Statuscheck", orderId);
            }
            return payresp;
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
        private async Task TokenGeneration(string baseUrl, string merchantId, string securityCode, bool refresh = false)
        {
            string url = "";
            if (!string.IsNullOrEmpty(baseUrl))
            {
                Uri myUri = new Uri(baseUrl);
                url = myUri.Scheme + "://" + myUri.Host;
            }
            string _request = string.Empty, _response = string.Empty;
            if (refresh)
            {
                var refreshReq = new
                {
                    AccessToken = TOKEN,
                    RefreshToken = TOKEN_REFRESH
                };
                url = url + "/token/refresh";
                _response = await AppWebRequest.O.PostJsonDataUsingHWRTLS(url, refreshReq);
                _request = JsonConvert.SerializeObject(refreshReq);
                goto Deserialization;
            }

        GenrateToken:
            var tokenReq = new
            {
                merchantID = merchantId,
                securityCode = securityCode
            };
            url = url + "/api/login";
            _response = await AppWebRequest.O.PostJsonDataUsingHWRTLS(url, tokenReq);
            _request = JsonConvert.SerializeObject(tokenReq);
        Deserialization:
            if (!string.IsNullOrEmpty(_response))
            {
                try
                {
                    var res = JsonConvert.DeserializeObject<AllUPIResponse<AllUPILoginResponse>>(_response);
                    if (res.statusCode == -2)
                        goto GenrateToken;
                    if (res.statusCode == 1)
                    {
                        EXPIRES_IN = 24;
                        TOKEN = res.result.token;
                        TOKEN_REFRESH = res.result.refreshToken;
                    }
                }
                catch (Exception ex)
                {
                    _response = "Exception:" + ex.Message + "||" + _response;
                }
            }
        }
    }
}