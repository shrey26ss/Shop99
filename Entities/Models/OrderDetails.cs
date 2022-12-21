using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class OrderDetailsRow
    {
        public int ID { get; set; }
        public int VarriantID { get; set; }
        public Guid OrderID { get; set; }
        public string Product { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public int Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal MRP { get; set; }
        public decimal Discount { get; set; }
        public decimal OtherCharge { get; set; }
        public decimal Total { get; set; }
        public int UserID { get; set; }
        public string EntryOn { get; set; }
        public StatusType StatusID { get; set; }
        public string Thumbnail { get; set; }
        public string ShopName { get; set; }
        public string Attributes { get; set; }
        public int PaymentModeId { get; set; }
        public string PaymentMode { get; set; }
    }
    public class OrderDetailsColumn : OrderDetailsRow
    {
    }
    public class OrderDetailsRequest
    {
        public int Id { get; set; }
        public StatusType StatusID { get; set; }
    }
    public class OrderShippedStatus
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please Enter Invoice Number")]
        [Display(Name ="Invoice Number")]
        public string InvoiceNumber { get; set; }
        [Required(ErrorMessage = "Please Enter TrackingId")]
        [Display(Name = "Tracking Id")]
        public string TrackingId { get; set; }
    }
}
