using Newtonsoft.Json;
using System.Collections.Generic;

namespace PaymentGateWay.PaymentGateway.CashFree
{
    public class Models
    {
        public class CashfreeCreateVACommon
        {
            public string name { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public string notifGroup { get; set; }
        }
        public class CashfreeCreateVAAccountRequest : CashfreeCreateVACommon
        {
            public string vAccountId { get; set; }
            public int createMultiple { get; set; }
        }
        public class CashfreeCreateVA_VPARequest : CashfreeCreateVACommon
        {
            public string virtualVpaId { get; set; }
        }

        public class CashfreeCreateAccountResp
        {
            public string status { get; set; }
            public string subCode { get; set; }
            public string message { get; set; }
            public CashfreeCreateAccountData data { get; set; }
        }
        public class CashfreeCreateAccountData
        {
            public string vpa { get; set; }
            public CashfreeCreateAccountDetail YESB { get; set; }
            public CashfreeCreateAccountDetail ICIC { get; set; }
            public CashfreeCreateAccountDetail IDFC { get; set; }
            public string vAccountId { get; set; }
            public string virtualAccountNumber { get; set; }
            public string ifsc { get; set; }
            public string virtualVPA { get; set; }
            public string accountNumber { get; set; }
        }
        public class CashfreeCreateAccountDetail
        {
            public string accountNumber { get; set; }
            public string ifsc { get; set; }
        }
        public class CashfreeCollectStatuscheckresp
        {
            public string status { get; set; }
            public string subCode { get; set; }
            public string message { get; set; }
            public CashfreeStatuscheckData data { get; set; }
        }
        public class CashfreeStatuscheckData
        {
            public CashfreeStatuscheckDataPayment payment { get; set; }
        }
        public class CashfreeStatuscheckDataPayment
        {
            public string vAccountId { get; set; }
            public string amount { get; set; }
            public int referenceId { get; set; }
            public string utr { get; set; }
            public string creditRefNo { get; set; }
            public string remitterAccount { get; set; }
            public string remitterName { get; set; }
            public string paymentTime { get; set; }
            public string isSettled { get; set; }
            public string settlementUtr { get; set; }
            public string settlementId { get; set; }
        }
        public class CashfreeColectResponse
        {
            public string amount { get; set; }
            public string creditrefno { get; set; }
            public string email { get; set; }
            [JsonProperty("event")]
            public string CashFreeEvent { get; set; }
            public string isVpa { get; set; }
            public string paymentTime { get; set; }
            public string phone { get; set; }
            public int referenceId { get; set; }
            public string remarks { get; set; }
            public string remitterAccount { get; set; }
            public string remitterIfsc { get; set; }
            public string remitterName { get; set; }
            public string remitterVpa { get; set; }
            public string signature { get; set; }
            public string transferType { get; set; }
            public string utr { get; set; }
            public string vAccountId { get; set; }
            public string vAccountNumber { get; set; }
        }
        public class CashFreeModels
        {
        }
        public class CashfreeOrderRequest
        {
            public string order_id { get; set; }
            public double order_amount { get; set; }
            public string order_currency { get; set; }
            public CustomerDetails customer_details { get; set; }
            public int payment_capture { get; set; }
            public OrderMeta order_meta { get; set; }
            public string order_expiry_time { get; set; }
        }
        public class CustomerDetails
        {
            public string customer_id { get; set; }
            public string customer_email { get; set; }
            public string customer_phone { get; set; }
            public string customer_bank_account_number { get; set; }
            public string customer_bank_ifsc { get; set; }
            public string customer_bank_code { get; set; }
        }
        public class CashfreeOrderResponse : CashfreeOrderRequest
        {
            public int cf_order_id { get; set; }
            public string entity { get; set; }
            public Settlements settlements { get; set; }
            public Payments payments { get; set; }
            public Refunds refunds { get; set; }
            public string order_status { get; set; }
            public string order_token { get; set; }
            public object order_note { get; set; }
            public string payment_link { get; set; }
            public object order_tags { get; set; }
            public List<object> order_splits { get; set; }
            public string code { get; set; }
            public string message { get; set; }
        }
        public class OrderMeta
        {
            public string return_url { get; set; }
            public string notify_url { get; set; }
            public string payment_methods { get; set; }
        }
        public class Settlements
        {
            public string url { get; set; }
        }
        public class Payments
        {
            public string url { get; set; }
        }
        public class Refunds
        {
            public string url { get; set; }
        }
        public class CashfreeCallbackResponse
        {
            public string orderId { get; set; }
            public decimal orderAmount { get; set; }
            public string referenceId { get; set; }
            public string txStatus { get; set; }
            public string paymentMode { get; set; }
            public string txMsg { get; set; }
            public string txTime { get; set; }
            public string signature { get; set; }
        }
        public class CashfreeStatusResponse
        {
            public string orderId { get; set; }
            public string OID { get; set; }
            public string orderStatus { get; set; }
            public string txStatus { get; set; }
            public string referenceId { get; set; }
            public string paymentMode { get; set; }
            public string orderCurrency { get; set; }
            public decimal orderAmount { get; set; }
            public string status { get; set; }
            public string reason { get; set; }
            public string TID { get; set; }
            public string TransactionId { get; set; }
            public string OrderToken { get; set; }
            public int StatusCode { get; set; }
            public string utr { get; set; }
            public PaymentDetails paymentDetails { get; set; }
        }
        public class PaymentDetails
        {
            public string payersVPA { get; set; }
            public string utr { get; set; }
        }
        public class CashFreePaymentMode
        {
            private readonly Dictionary<string, string> myDictionary;
            public CashFreePaymentMode()
            {
                myDictionary = GetDictionary();
            }

            private Dictionary<string, string> GetDictionary()
            {
                Dictionary<string, string> myDictionary = new Dictionary<string, string>();
                myDictionary.Add("CCRD", "cc");
                myDictionary.Add("DCR", "dc");
                myDictionary.Add("PWLT", "ppc");
                myDictionary.Add("PWLT", "nb");
                myDictionary.Add("PWLT", "upi");
                return myDictionary;
            }

            public string Mode(string key)
            {
                return myDictionary[key];
            }

        }
        public class CashfreeOrderRequestForApp
        {
            public string orderId { get; set; }
            public double orderAmount { get; set; }
            public string orderCurrency { get; set; }
        }
        public class CashFreeResponseForApp
        {
            public string status { get; set; }
            public string message { get; set; }
            public string cftoken { get; set; }
            public string appId { get; set; }
            public string orderId { get; set; }
            public double orderAmount { get; set; }
            public string orderCurrency { get; set; }
            public string customerEmail { get; set; }
            public string customerMobile { get; set; }
            public string notifyUrl { get; set; }
        }
        public class CashFreeAppSetting
        {
            public string BaseURL { get; set; }
            public string Host { get; set; }
            public string CLIENTID { get; set; }
            public string SECRETKEY { get; set; }
            public string CollectHost { get; set; }
            public string VirtualAccountURL { get; set; }
            public string CollectClientID { get; set; }
            public string CollectSecretKey { get; set; }

        }
        public class CFTGData
        {
            public string token { get; set; }
            public long expiry { get; set; }
        }
        public class CFTokenGen
        {
            public string status { get; set; }
            public string subCode { get; set; }
            public string message { get; set; }
            public CFTGData data { get; set; }
        }
        public class CFAVData
        {
            public string nameAtBank { get; set; }
            public string amountDeposited { get; set; }
            public string refId { get; set; }
            public string bankName { get; set; }
            public string utr { get; set; }
            public string city { get; set; }
            public string branch { get; set; }
            public int micr { get; set; }
        }

        public class CFAccVerify
        {
            public string status { get; set; }
            public string subCode { get; set; }
            public string message { get; set; }
            public string accountStatus { get; set; }
            public string accountStatusCode { get; set; }
            public CFAVData data { get; set; }
        }
        public class CFPAddBeneReq
        {
            public string beneId { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string bankAccount { get; set; }
            public string ifsc { get; set; }
            public string address1 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string pincode { get; set; }
            public string vpa { get; set; }
        }
        public class CFPAddBeneResp
        {
            public string status { get; set; }
            public string subCode { get; set; }
            public string message { get; set; }
        }

        public class QRCodeCashfreeRes
        {
            public string status { get; set; }
            public string subCode { get; set; }
            public string message { get; set; }
            public string qrCode { get; set; }
            public string virtualVPA { get; set; }
        }

        public class CFGBIDData
        {
            public string beneId { get; set; }
        }

        public class CFBeneIDResp
        {
            public string status { get; set; }
            public string subCode { get; set; }
            public string message { get; set; }
            public CFGBIDData data { get; set; }
        }
        public class CFAccTRData
        {
            public string referenceId { get; set; }
        }
        public class CFAccTRResp
        {
            public string status { get; set; }
            public string subCode { get; set; }
            public string message { get; set; }
            public CFAccTRData data { get; set; }
        }

        public class CFStsChkDataTransfer
        {
            public string transferId { get; set; }
            public string beneId { get; set; }
            public string amount { get; set; }
            public string status { get; set; }
            public string addedOn { get; set; }
            public string processedOn { get; set; }
            public string reason { get; set; }
            public string transferMode { get; set; }
            public string utr { get; set; }
            public int acknowledged { get; set; }
        }

        public class CFStsChkData
        {
            public CFStsChkDataTransfer transfer { get; set; }
        }

        public class CFStsChkResp
        {
            public string status { get; set; }
            public string subCode { get; set; }
            public string message { get; set; }
            public CFStsChkData data { get; set; }
        }

        #region CFCallBackResp
        public class CFOrder
        {
            public string order_id { get; set; }
            public double order_amount { get; set; }
            public string order_currency { get; set; }
            public object order_tags { get; set; }
        }

        public class CFUpi
        {
            public object channel { get; set; }
            public string upi_id { get; set; }
        }

        public class CFPaymentMethod
        {
            public CFUpi upi { get; set; }
        }

        public class CFPayment
        {
            public int cf_payment_id { get; set; }
            public string payment_status { get; set; }
            public double payment_amount { get; set; }
            public string payment_currency { get; set; }
            public string payment_message { get; set; }
            public string payment_time { get; set; }
            public string bank_reference { get; set; }
            public object auth_id { get; set; }
            public CFPaymentMethod payment_method { get; set; }
            public string payment_group { get; set; }
        }

        public class CFCustomerDetails
        {
            public object customer_name { get; set; }
            public string customer_id { get; set; }
            public string customer_email { get; set; }
            public string customer_phone { get; set; }
        }

        public class CFData
        {
            public CFOrder order { get; set; }
            public CFPayment payment { get; set; }
            public CFCustomerDetails customer_details { get; set; }
        }

        public class CFRealCallbackResp
        {
            public CFData data { get; set; }
            public string event_time { get; set; }
            public string type { get; set; }
        }
        #endregion
    }
}
