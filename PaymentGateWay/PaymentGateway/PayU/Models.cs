using Newtonsoft.Json;
using Service.Models;
using System.Collections.Generic;

namespace PaymentGateWay.PaymentGateway.PayU
{
    public class PayUTokenRequest
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string grant_type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string scope { get; set; }
    }
    public class PayUTokenResponse
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
        public long created_at { get; set; }
        public string user_uuid { get; set; }
    }
    public class PayUSetting
    {
        public string AUTHURL { get; set; }
        public string BASEURL { get; set; }
        public string PASSWORD { get; set; }
        public string ClientSecret { get; set; }
        public string PAYOUTID { get; set; }
        public string USERNAME { get; set; }
        public string CLIENTID { get; set; }
    }
    public class PayUPayoutRequest
    {
        public string beneficiaryAccountNumber { get; set; }
        public string beneficiaryIfscCode { get; set; }
        public string beneficiaryName { get; set; }
        public string beneficiaryEmail { get; set; }
        public string beneficiaryMobile { get; set; }
        public string purpose { get; set; }
        public int amount { get; set; }
        public string batchId { get; set; }
        public string merchantRefId { get; set; }
        public string paymentType { get; set; }
    }
    public class PayUDMTRequest
    {
        public string agentId { get; set; }
        public string amount { get; set; }
        public string bankName { get; set; }
        public string beneAccNo { get; set; }
        public string beneMobNo { get; set; }
        public string beneName { get; set; }
        public string customerMobile { get; set; }
        public string distTxnRefNo { get; set; }
        public string ifsc { get; set; }
        public string stateCode { get; set; }
        public string custFirstName { get; set; }
        public string custLastName { get; set; }
        public string custPincode { get; set; }
        public string custAddress { get; set; }
        public string field1 { get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
    }
    public class PayUPayoutResponse
    {
        public int? status { get; set; }
        public string msg { get; set; }
        public List<PayUPayoutData> data { get; set; }
    }
    public class PayUDMTResponse
    {
        public string code { get; set; }
        public string description { get; set; }
        public int? status { get; set; }
        public PayUResult result { get; set; }
    }
    public class PayUResult
    {
        public string distTxnRefNo { get; set; }
        public string payuTxnRefNo { get; set; }
        public string bankRefNo { get; set; }
        public string amount { get; set; }
        public string charges { get; set; }
        public string txnStatus { get; set; }
        public string txnCode { get; set; }
        public string beneName { get; set; }
        public string errorCode { get; set; }
        public string errorMsg { get; set; }
    }
    public class PayUPayoutData
    {
        public string batchId { get; set; }
        public string merchantRefId { get; set; }
        public string error { get; set; }
        public int[] code { get; set; }
    }
    public class PayUStatusCheckRequest
    {
        public string txnId { get; set; }
        public string merchantRefId { get; set; }
        public string batchId { get; set; }
        public string transferStatus { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }
    public class PayUStatusReponse
    {
        public int? status { get; set; }
        public string msg { get; set; }
        public PayUTransactionData data { get; set; }
    }
    public class PayUTransactionData
    {
        public int noOfPages { get; set; }
        public int totalElements { get; set; }
        public int currentPage { get; set; }
        public List<PayUTransactionDetail> transactionDetails { get; set; }
    }
    public class PayUTransactionDetail
    {
        public string txnId { get; set; }
        public string batchId { get; set; }
        public string merchantRefId { get; set; }
        public string purpose { get; set; }
        public decimal amount { get; set; }
        public string txnStatus { get; set; }
        public string txnDate { get; set; }
        public string payuTransactionRefNo { get; set; }
        public string beneficiaryName { get; set; }
        public string msg { get; set; }
        public string responseCode { get; set; }
        public string transferType { get; set; }
        public string bankTransactionRefNo { get; set; }
    }
    public class PayUVerifyAccountResponse
    {
        public int? status { get; set; }
        public string msg { get; set; }
        public PayUVerifyData data { get; set; }
    }
    public class PayUVerifyData
    {
        public string accountExists { get; set; }
        public string beneficiaryName { get; set; }
    }

    public class PayUBillFetchRequestModel
    {
        public int MyProperty { get; set; }
    }
    public class PayuAppBBPSSetting
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string grant_type { get; set; }
        public string scope { get; set; }
        public string TokenAPIURL { get; set; }
        public string BillerDetailByCatURL { get; set; }
        public string FetchBillURL { get; set; }
        public string BillPaymentURL { get; set; }
        public string ComplainURL { get; set; }
        public string ComplainStatusURL { get; set; }
        public string StatusCheckURL { get; set; }
    }
    public class PayUBillCommonRes
    {
        public int code { get; set; }
        public string status { get; set; }
    }
    public class PayUBillerResponse : PayUBillCommonRes
    {
        public PayUPayloadBiller payload { get; set; }
    }
    public class PayUPayloadBiller
    {
        public List<PayuBiller> billers { get; set; }
    }
    public class PayUComplainResp : PayUBillCommonRes
    {
        public PayUPayloadComplain payload { get; set; }
    }
    public class PayUPayloadComplain
    {
        public string refId { get; set; }
        public string complaintStatus { get; set; }
        public string complaintId { get; set; }
        public string billerReply { get; set; }
        public string billerID { get; set; }
        public List<PayUError> errors { get; set; }
    }
    public class PayuBiller
    {
        public string billerId { get; set; }
        public string billerName { get; set; }
        public List<PayuPayuCustomerParam> customerParams { get; set; }
        public bool isAdhoc { get; set; }
        public string fetchOption { get; set; }
        public string regionCode { get; set; }
        public string state { get; set; }
        public string flowType { get; set; }
        public List<PayuAllowedPaymentMode> paymentModesAllowed { get; set; }
        public string billerOwnerShp { get; set; }
        public string status { get; set; }
        public string billerEffctvFrom { get; set; }
        public string billerEffctvTo { get; set; }
        public string billerMode { get; set; }
        public string billerAliasName { get; set; }
        public string paymentAmountExactness { get; set; }
        public string supportBillValidation { get; set; }
        public bool supportDeemed { get; set; }
        public bool supportPendingStatus { get; set; }
        public PayUBlrResponseParams blrResponseParams { get; set; }
        public List<PayUPaymentChannelsAllowed> paymentChannelsAllowed { get; set; }

        public List<PayuOperatorDictionary> OperatorDictionary { get; set; }
    }
    public class PayUPaymentChannelsAllowed
    {
        public string paymentMode { get; set; }
        public double maxLimit { get; set; }
        public double minLimit { get; set; }
    }
    public class PayUBlrResponseParams
    {
        public List<PayUAmountOPtion> amountOptions { get; set; }
    }
    public class PayUAmountOPtion
    {
        public List<string> amountBreakupSet { get; set; }
    }
    public class PayuPayuCustomerParam
    {
        public string regex { get; set; }
        public bool visibility { get; set; }
        public string dataType { get; set; }
        public int minLength { get; set; }
        public bool optional { get; set; }
        public string paramName { get; set; }
        public int maxLength { get; set; }
    }

    public class PayuOperatorDictionary
    {
        public int ParamID { get; set; }
        public int Ind { get; set; }
        public int OID { get; set; }
        public string DropDownValue { get; set; }
    }
    public class PayuAllowedPaymentMode
    {
        public string supportPendingStatus { get; set; }
        public string paymentMode { get; set; }
        public float minLimit { get; set; }
    }
    public class PayUBillFetchResponse : PayUBillCommonRes
    {
        public PayUBillFetchPayload payload { get; set; }
    }
    public class PayUBillFetchPayload
    {
        public string refId { get; set; }
        public string requestTimeStamp { get; set; }
        public double amount { get; set; }
        public string accountHolderName { get; set; }
        public string dueDate { get; set; }
        public string billDate { get; set; }
        public string billerId { get; set; }
        public object amountDetails { get; set; }
        public List<PayUError> errors { get; set; }

        public dynamic additionalParams { get; set; }
    }
    public class PayUPaymentResponse : PayUBillCommonRes
    {
        public PayUPayloadBillPayment payload { get; set; }
    }
    public class PayUPaymentDetail
    {
        public string paymentMode { get; set; }

        [JsonProperty("params")]
        public object ParamsPayMode { get; set; }
    }
    public class PayUPayloadBillPayment
    {
        public string requestTimeStamp { get; set; }
        public double paidAmount { get; set; }
        public string refId { get; set; }
        public string message { get; set; }
        public string billerId { get; set; }
        public PayUAdditionalParams additionalParams { get; set; }
    }
    public class PayUError
    {
        public string reason { get; set; }
        public string errorCode { get; set; }
    }
    public class PayUAdditionalParams
    {
        public string txnReferenceId { get; set; }
    }

    public class PayURequest
    {
        public string key { get; set; }//merchant  key
        public string salt { get; set; }//merchant  key
        public string txnid { get; set; }
        public double amount { get; set; }
        public string productinfo { get; set; }
        public string firstname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string surl { get; set; }
        public string furl { get; set; }
        public string hash { get; set; }
        public string enforce_paymethod { get; set; }
        public string udf5 { get; set; }
        public string service_provider { get; set; }
    }

    public class PayUResponse
    {
        public string mihpayid { get; set; }// unique reference number
        public string mode { get; set; }
        public string status { get; set; }
        public string unmappedstatus { get; set; }
        public string key { get; set; }
        public string txnid { get; set; }
        public double amount { get; set; }
        public string cardCategory { get; set; }
        public double discount { get; set; }
        public double net_amount_debit { get; set; }
        public string addedon { get; set; }
        public string productinfo { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string udf1 { get; set; }
        public string udf2 { get; set; }
        public string udf3 { get; set; }
        public string udf4 { get; set; }
        public string udf5 { get; set; }
        public string hash { get; set; }
        public string field1 { get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
        public string field4 { get; set; }
        public string field5 { get; set; }
        public string field6 { get; set; }
        public string field7 { get; set; }
        public string field8 { get; set; }
        public string field9 { get; set; }
        public string payment_source { get; set; }
        public string PG_TYPE { get; set; }
        public string bank_ref_num { get; set; }
        public string bankcode { get; set; }
        public string error { get; set; }
        public string error_Message { get; set; }
        public string name_on_card { get; set; }
        public string cardnum { get; set; }

    }

    public class PayUStatusCheckResponse
    {
        public int status { get; set; }
        public int msg { get; set; }
        public PayUResponse transaction_details { get; set; }
    }
    public class PayUVerifyRequest
    {
        public string key { get; set; }
        public string command { get; set; }
        public string var1 { get; set; }
        public string hash { get; set; }
    }
    public class PayUGUpdate 
    {
        public int TID { get; set; }
        public int Amount { get; set; }
        public string Hash { get; set; }
        public PayUResponseCallback PayUCallbackResp { get; set; }
    }
    public class PayUResponseCallback
    {
        public string Status { get; set; }// unique reference number
        public string CHECKSUMHASH { get; set; }
        public string BANKNAME { get; set; }
        public string ORDERID { get; set; }
        public string TXNAMOUNT { get; set; }
        public string TXNDATE { get; set; }
        public string MID { get; set; }

        public string TXNID { get; set; }
        public string RESPCODE { get; set; }
        public string PAYMENTMODE { get; set; }
        public string BANKTXNID { get; set; }
        public string CURRENCY { get; set; }
        public string GATEWAYNAME { get; set; }
        public string RESPMSG { get; set; }

    }
    public class PayuStatusRes
    {
        public int status { get; set; }
        public string msg { get; set; }
        public dynamic transaction_details { get; set; }

        public string TranStatus { get; set; }
        public string LiveID { get; set; }
        public string Amount { get; set; }
        public string PaymentMode { get; set; }

    }
    public class Payures
    {
        public Payuresdata Payuresdata { get; set; }
    }
    public class Payuresdata
    {
        public string mihpayid { get; set; }
        public string request_id { get; set; }
        public string bank_ref_num { get; set; }
        public string amt { get; set; }
        public string transaction_amount { get; set; }
        public string txnid { get; set; }
        public string additional_charges { get; set; }
        public string productinfo { get; set; }
        public string firstname { get; set; }
        public string bankcode { get; set; }
        public object udf1 { get; set; }
        public object udf3 { get; set; }
        public object udf4 { get; set; }
        public object udf5 { get; set; }
        public string field2 { get; set; }
        public string field9 { get; set; }
        public string error_code { get; set; }
        public string addedon { get; set; }
        public string payment_source { get; set; }
        public string card_type { get; set; }
        public string error_Message { get; set; }
        public string net_amount_debit { get; set; }
        public string disc { get; set; }
        public string mode { get; set; }
        public string PG_TYPE { get; set; }
        public string card_no { get; set; }
        public string name_on_card { get; set; }
        public object udf2 { get; set; }
        public string status { get; set; }
        public string unmappedstatus { get; set; }
        public object Merchant_UTR { get; set; }
        public string Settled_At { get; set; }
    }

}
