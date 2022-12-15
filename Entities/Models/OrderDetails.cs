using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class OrderDetailsRow
    {
        public int ID { get; set; }
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
        public StausType StatusID { get; set; }
        public string Thumbnail { get; set; }
        public string ShopName { get; set; }
        public string Attributes { get; set; }
    }
    public class OrderDetailsColumn : OrderDetailsRow
    {
    }
}
