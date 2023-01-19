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
        public string Remark { get; set; }
        public string InvoiceNumber { get; set; }
    }
    public class OrderDetailsColumn : OrderDetailsRow
    {
        public string DocketNo { get; set; }
        public DateTime ReturnTillOn { get; set; }
    }
    public class OrderDetailsRequest
    {
        public int Id { get; set; }
        public StatusType StatusID { get; set; }
        public int Top { get; set; } = 50;
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
    public class OrderInvoiceRequest
    {
        public int OrderId { get; set; }
    }
    public class OrderInvoice
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string ShippingAddress { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime OrderDate { get; set; }
        public string Title { get; set; }
        public decimal Rate { get; set; }
        public decimal MRP { get; set; }
        public string DocketNo { get; set; }
        public string Attributes { get; set; }
        public string ContactNo { get; set; }
        public string ShopName { get; set; }
        public string VendorAddress { get; set; }
        public string VendorState { get; set; }
        public string VendorEmail { get; set; }
        public int Qty { get; set; }
    }
    public class OrderReplacedConformReq
    {
        public int ID { get; set; }
        public string Role { get; set; }
    }
    public class ReturnRequestList
    {
        public int ID { get; set; }
        public string Thumbnail { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string StatusType { get; set; }
        public int Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal MRP { get; set; }
        public string PaymentMode { get; set; }
        public string EntryOn { get; set; }
    }
}
