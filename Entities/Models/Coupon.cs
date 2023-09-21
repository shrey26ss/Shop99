using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Coupon
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public bool IsFixed { get; set; }
        public decimal DiscountAmount { get; set; }
        public string EntryOn { get; set; }
        public string ExpiryOn { get; set; }
        public bool IsActive { get; set; }
        public string PaymentModes { get; set; }
        public bool IsWelcomeCoupon { get; set; }
        [MaxLength(20)]
        public string Description { get; set; }
        public decimal MaxBenefit { get; set; }
        public int UseCount { get; set; }
        public bool IsProductDependent { get; set; }
        public decimal MinPurchaseForRedeem { get; set; }
    }
    public class CouponUpdateIsActive
    {
        public int CouponId { get; set; }
        public bool IsActive { get; set; }
        public bool IsFixed { get; set; }
        public bool IsWelcomeCoupon { get; set; }
        public bool IsProductDependent { get; set; }
    }
}
