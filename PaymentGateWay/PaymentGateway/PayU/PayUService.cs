using AppUtility.APIRequest;
using AppUtility.Helper;
using AutoMapper;
using Data;
using Entities;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateWay.PaymentGateway.MitraUPI;
using PaymentGateWay.PaymentGateway.PayU;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateWay.PaymentGateway.PayU
{
    public class PayUService: PaymentGatewayBase
    {
        //*Note : MerchantId --> PayU Salt  & Merchant Key or key --> Alieas

        private readonly IMapper _mapper;
        private IDapperRepository _dapper;
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
        public PayUService(ILogger logger, IDapperRepository dapper, IMapper mapper, IAPILogger aPILogin) : base(logger, dapper)
        {
            _mapper = mapper;
            _apiLogin = aPILogin;
            _dapper = dapper;
        }

        public  async Task<ResponsePG<PaymentGatewayResponse>> GeneratePGRequestForWeb(PaymentGatewayRequest request)
        {
            ResponsePG<PaymentGatewayResponse> res = new ResponsePG<PaymentGatewayResponse>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            string paymentMode = string.Empty;
            if (paymentModes.ContainsKey(request.PaymentModeShortName))
            {
                paymentMode = paymentModes[request.PaymentModeShortName];
            }
            var payURequest = new PayURequest
            {
                key = request.MerchantKey,
                amount = (double)request.Amount,
                txnid = "TID" + request.TID,
                surl = request.Domain + "/PGCallback/PayUnotify",
                furl = request.Domain + "/PGCallback/PayUnotify",
                firstname = request.UserID.ToString(),
                email = request.EmailID,
                phone = request.MobileNo,
                enforce_paymethod = paymentMode,
                productinfo = "Add Money"
            };
            try
            {

               
                Dictionary<string, string> keyValue = new Dictionary<string, string>(){
                    {"key", payURequest.key},
                    {"txnid", payURequest.txnid},
                    {"amount", payURequest.amount.ToString()},
                    {"firstname", payURequest.firstname},
                    {"email", payURequest.email},
                    {"phone", payURequest.phone},
                    {"productinfo", payURequest.productinfo},
                    {"surl", payURequest.surl},
                    {"furl", payURequest.furl},
                    {"enforce_paymethod", payURequest.enforce_paymethod},
                };
                payURequest.hash = GenerateHash(request.MerchantID,
                    new List<string> { payURequest.key,
                        payURequest.txnid,
                        payURequest.amount.ToString(),
                        payURequest.productinfo,
                        payURequest.firstname,
                        payURequest.email,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty });//keyValue
                keyValue.Add("hash", payURequest.hash.ToLower());
                res.KeyVals = keyValue;
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "Transaction intiated";
                //savePGTransactionLog(request.PGID, request.TID, JsonConvert.SerializeObject(res), request.TransactionID, payURequest.hash, RequestMode.PANEL, true, request.Amount, string.Empty);
            }
            catch (Exception ex)
            {
                res.StatusCode = ResponseStatus.Failed;
                res.ResponseText = ex.Message;
                string errorMsg = string.Concat(ex.Message, " | request : ", JsonConvert.SerializeObject(payURequest), " | response : ", JsonConvert.SerializeObject(res));
                LogError(errorMsg, this.GetType().Name, nameof(this.GeneratePGRequestForWeb));
            }
            if (request.IsLoggingTrue)
            {
                _apiLogin.SaveLog(JsonConvert.SerializeObject(payURequest), JsonConvert.SerializeObject(res), "PayU", request.TID);
            }
            return res;
        }
      

        private string GenerateHashPayUApp(string salt, List<string> keyValuePairs)
        {
            var sb = new StringBuilder();
            sb.Append(salt);
            if (keyValuePairs != null)
            {
                foreach (var item in keyValuePairs)
                {
                    sb.Append(item);
                    sb.Append("|");
                }
            }
            string str = sb.ToString();

            string str1 = str.Remove(str.Length - 1, 1);

            return HashEncryption.O.SHA512Hash(str1);
        }
        
        private string GenerateHash(string salt, List<string> keyValuePairs)
        {
            var sb = new StringBuilder();
            if (keyValuePairs != null)
            {
                foreach (var item in keyValuePairs)
                {
                    sb.Append(item);
                    sb.Append("|");
                }
            }
            sb.Append("|||||");
            sb.Append(salt);
            string str = sb.ToString();
            return HashEncryption.O.SHA512Hash(str);
        }
        public async Task<ResponsePG<StatusCheckResponse>> StatusCheckPG(StatusCheckRequest request)
        {
            var payuResponse = new PayuStatusRes();
            var Payures = new Payures();
            ResponsePG<StatusCheckResponse> res = new ResponsePG<StatusCheckResponse>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString(),
                Result = new StatusCheckResponse()
            };
            string payuRes = string.Empty;
            var payuVerifyRequest = new PayUVerifyRequest();
            PaymentGatewayModel pgConfig = new PaymentGatewayModel();
            pgConfig = await _dapper.GetAsync<PaymentGatewayModel>("select * from PaymentGatwaydetails where PGId = @PGID",new { request.PGID},System.Data.CommandType.Text);
            StringBuilder sb = new StringBuilder("key={key}&command={command}&var1={var1}&hash={hash}");
            sb.Replace("{key}", pgConfig.MerchantKey);
            sb.Replace("{command}", "verify_payment");
            sb.Replace("{var1}", "TID" + request.TID.ToString());
            sb.Replace("{hash}", GenerateHashPayU(pgConfig.MerchantID, new List<string> { pgConfig.MerchantKey, "verify_payment", "TID" + request.TID.ToString() }));
            try
            {
                //sb.Replace("{hash}", GenerateCheckSum(this.salt, new List<string> { request.MerchantKEY, "verify_payment", request.TID.ToString() }));
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "Statuscheck";
                payuRes = AppWebRequest.O.CallUsingHttpWebRequest_POST(pgConfig.StatusCheckURL, sb.ToString());

                if (!string.IsNullOrEmpty(payuRes))
                {

                    payuResponse = JsonConvert.DeserializeObject<PayuStatusRes>(payuRes);
                    string vall = Convert.ToString(payuResponse.transaction_details);
                    String newStr = vall.Remove(4, 13);
                    //string strRes = newStr.Insert(3, "Payuresdata:");
                    string strRes = newStr.Insert(3, "Payuresdata");
                    strRes = strRes.Replace("\r\n", "");
                    Payures = JsonConvert.DeserializeObject<Payures>(strRes);
                    if (payuResponse.status == 1)
                    {
                        payuResponse.TranStatus = Payures.Payuresdata.status;
                        payuResponse.LiveID = Payures.Payuresdata.bank_ref_num;
                        payuResponse.Amount = Payures.Payuresdata.amt;
                        payuResponse.PaymentMode = Payures.Payuresdata.mode;
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = string.Concat(ex.Message, " | request : ", JsonConvert.SerializeObject(sb.ToString()), " | response : ", JsonConvert.SerializeObject(res.Result));
                LogError(errorMsg, this.GetType().Name, nameof(this.StatusCheck));

            }
            if (pgConfig.IsLoggingTrue)
            {
                _apiLogin.SaveLog(string.Concat(pgConfig.StatusCheckURL, "|", sb.ToString()), JsonConvert.SerializeObject(payuRes), "PayU-->Statuscheck", payuResponse.LiveID);
            }

            return res;
        }
        private string GenerateHashPayU(string salt, List<string> keyValuePairs)
        {
            var sb = new StringBuilder();
            if (keyValuePairs != null)
            {
                foreach (var item in keyValuePairs)
                {
                    sb.Append(item);
                    sb.Append("|");
                }
            }

            sb.Append(salt);
            string str = sb.ToString();
            return HashEncryption.O.SHA512Hash(str);
        }
    }
}
