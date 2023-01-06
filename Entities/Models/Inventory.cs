using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class ReportRow
    {

    }
    public class ReportColumn
    {

    }
    public class Inventory
    {
        public int Id { get; set; }
        public int VarriantId { get; set; }
        public PurchaseType IsOut { get; set; }
        public int OpeningQty { get; set; }
        public int Qty { get; set; }
        public int ClosingQty { get; set; }
        public string Remark { get; set; }
        public int RefferenceId { get; set; }
        public string ProductName { get; set; }
        public string VariantTitle { get; set; }
    }
    public class InventoryRequest
    {
        public InventoryStatus Status { get; set; } = 0;
    }
}
