using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class GetOffers
    {
        public int OfferId { get; set; }
        public int OfferTypeId { get; set; }
        public string OfferTitle { get; set; }
        public string OfferDescription { get; set; }
        public string OfferBeginOn { get; set; }
        public string OfferEndOn { get; set; }
        public decimal DiscountUpToAmount { get; set; }
        public decimal AtRate { get; set; }
        public decimal MinEligibleAmount { get; set; }
        public int UserID { get; set; }
        public string CouponCode { get; set; }
        public bool IsActive { get; set; }
    }
    public class offerstype
    {
        public int Id { get; set; }
        public string OfferName { get; set; }
        public bool IsActive { get; set; }
    }
    public class OfferViewModel : GetOffers
    {
        public List<offerstype> offerstypeDDLs { get; set; }
    }
    public class OfferUpdateIsActive
    {
        public int OfferID { get; set; }
        public bool IsActive { get; set; }
    }
}
