
using Entities.Enums;
using System.Collections.Generic;

namespace Entities
{
    public class PaymentGatewayModel
    {
        public int Id { get; set; }
        public PaymentGatewayType PGId { get; set; }
        public string BaseURL { get; set; }
        public string MerchantID { get; set; }
        public string MerchantKey { get; set; }
        public string SuccessURL { get; set; }
        public string FailURL { get; set; }
        public string StatusCheckURL { get; set; }
        public bool IsLive { get; set; }
        public bool IsLoggingTrue { get; set; }
        public string VPA { get; set; }
        public bool IsActive { get; set; }

    }
    public class PGDisplayModel
    {
        public int PackageId { get; set; }
        public IEnumerable<PaymentGatewayType> PGs { get; set; }
    }

}
