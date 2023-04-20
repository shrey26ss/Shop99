using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateWay.PaymentGateway.PayU
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class PostBackParam
    {
        public int postBackParamId { get; set; }
        public string mihpayid { get; set; }
        public int paymentId { get; set; }
        public string mode { get; set; }
        public string status { get; set; }
        public string unmappedstatus { get; set; }
        public string key { get; set; }
        public string txnid { get; set; }
        public string amount { get; set; }
        public string additionalCharges { get; set; }
        public string addedon { get; set; }
        public long createdOn { get; set; }
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
        public string udf6 { get; set; }
        public string udf7 { get; set; }
        public string udf8 { get; set; }
        public string udf9 { get; set; }
        public string udf10 { get; set; }
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
        public string bank_ref_num { get; set; }
        public string bankcode { get; set; }
        public string error { get; set; }
        public string error_Message { get; set; }
        public string cardToken { get; set; }
        public string offer_key { get; set; }
        public string offer_type { get; set; }
        public string offer_availed { get; set; }
        public string pg_ref_no { get; set; }
        public string offer_failure_reason { get; set; }
        public string name_on_card { get; set; }
        public string cardnum { get; set; }
        public string cardhash { get; set; }
        public string card_type { get; set; }
        public object card_merchant_param { get; set; }
        public string version { get; set; }
        public string postUrl { get; set; }
        public bool calledStatus { get; set; }
        public string additional_param { get; set; }
        public string amount_split { get; set; }
        public string discount { get; set; }
        public string net_amount_debit { get; set; }
        public object fetchAPI { get; set; }
        public string paisa_mecode { get; set; }
        public string meCode { get; set; }
        public string payuMoneyId { get; set; }
        public object encryptedPaymentId { get; set; }
        public object id { get; set; }
        public object surl { get; set; }
        public object furl { get; set; }
        public object baseUrl { get; set; }
        public int retryCount { get; set; }
        public object merchantid { get; set; }
        public object payment_source { get; set; }
        public int isConsentPayment { get; set; }
        public bool giftCardIssued { get; set; }
        public object cardCategory { get; set; }
        public string pg_TYPE { get; set; }
        public bool s2SPbpFlag { get; set; }
    }

    public class TransactionDetails
    {
        public string merchantTransactionId { get; set; }
        public double transactionAmount { get; set; }
        public double merchantServiceFee { get; set; }
        public double merchantServiceTax { get; set; }
        public PostBackParam postBackParam { get; set; }
    }

    public class PayUNewResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public List<TransactionDetails> Result { get; set; }
        public object errorCode { get; set; }
        public object responseCode { get; set; }
    }


}
