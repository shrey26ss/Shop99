using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class PlaceOrderReq
    {
        public int UserID { get; set; }
        public int AddressID { get; set; }
        public int PaymentMode { get; set; }
        public bool IsBuyNow { get; set; }
        public string Remark { get; set; }
    }
    public class PlaceOrderResponse
    {
        public string OrderID { get; set; }
        public PaymentGatewayResponse pgResponse { get; set; }
        public ResponseStatus StatusCode { get; set; }
        public string ResponseText { get; set; }
    }
    public class PaymentGatewayResponse
    {
        public PaymentGatewayType PGType { get; set; }
        public string TID { get; set; }
        public string URL { get; set; }
        public Dictionary<string, string> KeyVals { get; set; }
        public dynamic APIResponse { get; set; }

        // public StatusCheckRequest chkreq { get; set; }
    }
    public class PaymentGatewayResponse<T> : PaymentGatewayResponse
    {
        public T Data { get; set; }
    }
    public class PaymentGatewayRequest
    {
        public ResponseStatus StatusCode { get; set; }
        public string ResponseText { get; set; }
        public int UserID { get; set; }
        public decimal Amount { get; set; }
        public string TID { get; set; }
        public string URL { get; set; }
        public string StatusCheckURL { get; set; }
        public PaymentGatewayType PGID { get; set; }
        public string MerchantID { get; set; }
        public string MerchantKey { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string Name { get; set; }
        public string PaymentModeShortName { get; set; }
        public string Domain { get; set; }
        public string VPA { get; set; }
        public bool IsLoggingTrue { get; set; }
        public bool IsPayment { get; set; }
    }
    public class StatusCheckResponse
    {
        public string OrderId { get; set; }
        public string OrderStatus { get; set; }
        public decimal OrderAmount { get; set; }
        public string ReferenceId { get; set; }
        public string PaymentMode { get; set; }
        public bool IsUpdateDb { get; set; }
        public dynamic APIResponse { get; set; }
    }

    public class StatusCheckRequest
    {
        public PaymentGatewayType PGID { get; set; }
        public int TID { get; set; }
    }

}
