using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Coupon
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public bool IsFixed { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0.")]
        public decimal DiscountAmount { get; set; }
        public string EntryOn { get; set; }
        [Required(ErrorMessage = "{0} is mandetory")]
        public string ExpiryOn { get; set; }
        public bool IsActive { get; set; }
        public string PaymentModes { get; set; }
        public bool IsWelcomeCoupon { get; set; }
        [MaxLength(150)]
        public string Description { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0.")]
        public decimal MaxBenefit { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0.")]
        public int UseCount { get; set; }
        public bool IsProductDependent { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0.")]
        public decimal MinPurchaseForRedeem { get; set; }
        public bool IsAutoApply { get; set; }
    }
    public class CouponUpdateIsActive
    {
        public int CouponId { get; set; }
        public bool IsActive { get; set; }
        public bool IsFixed { get; set; }
        public bool IsWelcomeCoupon { get; set; }
        public bool IsAutoApply { get; set; }
        public bool IsProductDependent { get; set; }
    }
}
