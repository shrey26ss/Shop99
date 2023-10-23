using Entities.Enums;

namespace Entities.Models
{
    public class InitiatePayment
    {
        public int TID { get; set; }
        public PaymentGatewayType PGID { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string PhoneNumber { get; set; }
        public string EntryOn { get; set; }
        public string ModifyOn { get; set; }
        public string Status { get; set; }
        public string UTR { get; set; }
        public int ReferenceId { get; set; }
    }

    

    public class InitiatePaymentRequest
    {
        public StatusType Status { get; set; }
    }
}
