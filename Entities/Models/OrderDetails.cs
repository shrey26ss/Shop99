using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class OrderDetailsRow
    {
        public string InvoiceNumber { get; set; }
        public string Product { get; set; }
        public string Title { get; set; }
        public StatusType StatusID { get; set; }
        public int Qty { get; set; }
        public string Attributes { get; set; }
        public decimal Rate { get; set; }
        public decimal MRP { get; set; }
        public string PaymentMode { get; set; }
        public string EntryOn { get; set; }
        public string ShopName { get; set; }
        public int ID { get; set; }
        public int VarriantID { get; set; }
        public Guid OrderID { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public decimal Discount { get; set; }
        public decimal OtherCharge { get; set; }
        public decimal Total { get; set; }
        public int UserID { get; set; }
        public string Thumbnail { get; set; }
        public int PaymentModeId { get; set; }
        public string Remark { get; set; }
        public string IGST { get; set; }
        public string SGST { get; set; }
        public string CGST { get; set; }
        public string ImagePaths { get; set; }
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
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string SearchText { get; set; } = string.Empty;
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
      
        public string ShopName { get; set; }
        public string VendorAddress { get; set; }
        public string VendorState { get; set; }
        public string VendorEmail { get; set; }
        public string VendorMobile { get; set; }
        public string GSTN { get; set; }
        public int Qty { get; set; }
        public double IGST { get; set; }
        public double SGST { get; set; }
        public double CGST { get; set; }
             
    }
    public class OrderReplacedConformReq
    {
        public int ID { get; set; }
        public string Role { get; set; }
        public string Remark { get; set; }
        public StatusType StatusID { get; set; }
    }
    public class ReturnRequestList
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public int Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal MRP { get; set; }
        public string StatusType { get; set; }
        public string PaymentMode { get; set; }
        public string EntryOn { get; set; }
        public int ID { get; set; }
        public string Thumbnail { get; set; }
        public string SourceImage { get; set; }
        public string Remark { get; set; }
    }
    public class UsersOrderTrakingRes
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string VendorName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Thumbnail { get; set; }
        public string Area { get; set; }
        public string Landmark { get; set; }
        public string TownCity { get; set; }
        public string CreatedOn { get; set; }
        public string StateName { get; set; }
        public double MRP { get; set; }
        public double SellingCost { get; set; }
        public StatusType StatusID { get; set; }
        public int Pincode { get; set; }
        public string HouseNo { get; set; }
    }
    public class OrderTimeline
    {
        public int Id { get; set; }
        public string StatusType { get; set; }
        public string EntryOn { get; set; }
    }
    public class UsersOrderTrakingViewModel
    {
        public UsersOrderTrakingRes usersOrderTrakingRes { get; set; }
        public IEnumerable<OrderTimeline> OrderTimeline { get; set; }
    }
    public class OrderGSTDetails
    {
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string VoucherType { get; set; }
        public string CustomerName { get; set; }
        public string BillingState { get; set; }
        public string ItemName { get; set; }
        public string HSNCode { get; set; }
        public Int32 GSTRate { get; set; }
        public Int32 Quantity { get; set; }
        public Int32 Rate { get; set; }
        public Int32 Amount { get; set; }
        public Int32 IGST { get; set; }
        public Int32 CGST { get; set; }
        public Int32 SGST { get; set; }
    }
}
