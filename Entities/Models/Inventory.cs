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
        public bool IsOut { get; set; }
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
        public StatusType Status { get; set; } = 0;
    }
    public class NewsLatter
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CeratedOn { get; set; }
        public string Email { get; set; }
    }
}
