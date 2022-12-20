using Entities.Enums;
using Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class Response<T> : IResponse<T>
    {
        public ResponseStatus StatusCode { get; set; }
        public string ResponseText { get; set; }
        public Exception Exception { get; set; }
        public T Result { get; set; }
        public Dictionary<string, string> KeyVals { get; set; }

        public Response()
        {
            StatusCode = ResponseStatus.Failed;
            ResponseText = ResponseStatus.Failed.ToString();
        }
    }

    public class Response : IResponse
    {
        public ResponseStatus StatusCode { get; set; } = ResponseStatus.Failed;
        public string ResponseText { get; set; } = ResponseStatus.Failed.ToString();
        public Response()
        {
            this.StatusCode = ResponseStatus.Failed;
            this.ResponseText = ResponseStatus.Failed.ToString();
        }
    }
    public class Request<T> : IRequest<T>
    {
        public string AuthToken { get; set; }
        public T Param { get; set; }
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
        public int UserID { get; set; }
        public decimal Amount { get; set; }
        public string TID { get; set; }
        public string TransactionID { get; set; }
        public string PGName { get; set; }
        public string URL { get; set; }
        public string StatusCheckURL { get; set; }
        public PaymentGatewayType PGID { get; set; }
        public string MerchantID { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string MerchantKey { get; set; }
        public string ENVCode { get; set; }
        public string IndustryType { get; set; }
        public string SuccessURL { get; set; }
        public string FailedURL { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public int WID { get; set; }
        public string OPID { get; set; }
        public string Domain { get; set; }
        public string VPA { get; set; }
        public bool IsLive { get; set; }
        public bool IsLoggingTrue { get; set; }
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
