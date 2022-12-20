using Service.Models;

namespace PaymentGateWay.PaymentGateway.MitraUPI
{
    public class PGTransactionParam : PGTransactionResponse
    {
        public string VendorID { get; set; }
        public string LiveID { get; set; }
        public string Remark { get; set; }
        public string PAYMENTMODE { get; set; }
        public string Checksum { get; set; }
        public string Signature { get; set; }
        public int UPGID { get; set; }
        public int RequestMode { get; set; }
        public int Status { get; set; }
        public int ReffID { get; set; }
    }
    public class PGTransactionResponse
    {
        public int Statuscode { get; set; }
        public string Msg { get; set; }
        public int UserID { get; set; }
        public decimal Amount { get; set; }
        public decimal RequestedAmount { get; set; }
        public int TID { get; set; }
        public string TransactionID { get; set; }
        public string PGName { get; set; }
        public string URL { get; set; }
        public string StatusCheckURL { get; set; }
        public int PGID { get; set; }
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
        public bool IsIntentAllowed { get; set; }
    }
    public class AllUPIStatusResponse : Response
    {
        public string status { get; set; }
        public string requestedId { get; set; }
        public decimal amount { get; set; }
        public string tr { get; set; }
        public int tid { get; set; }
        public string liveId { get; set; }
        public string utr { get; set; }
    }

    public class AllUPIRequest
    {
        public string requestedId { get; set; }
        public decimal amount { get; set; }
        public string upiId { get; set; }
        public string serverHookURL { get; set; }
        public string webHookURL { get; set; }
        public bool IsSelfCall { get; set; }
    }
    public class AllUPIInitiateTransactionResponse
    {
        public int statusCode { get; set; }
        public string responseText { get; set; }
        public int transactionId { get; set; }
        public string url { get; set; }
        public string intentString { get; set; }
    }

    public class AllUPISetting
    {
        public string BASEURL { get; set; }
        public string AuthEnd { get; set; }
        public string refreshTokenEnd { get; set; }
    }

    public class AllUPILoginResponse
    {
        public int id { get; set; }
        public string username { get; set; }
        public string role { get; set; }
        public string token { get; set; }
        public string refreshToken { get; set; }
        public object name { get; set; }
    }

    public class AllUPIResponse<T>
    {
        public int statusCode { get; set; }
        public string responseText { get; set; }
        public object exception { get; set; }
        public T result { get; set; }
    }
}
