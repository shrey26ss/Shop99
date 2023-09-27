
using Entities.Enums;

namespace Entities.Models
{
    public class RequestBase<T> : Request
    {
        public T Data { get; set; }
    }
    public class Request
    {
        public int LoginId { get; set; } = 0;
        public int RoleId { get; set; } = 0;
        public int Ind { get; set; } = 0;
    }
    public class SearchItem
    {
        public bool IsProductDependent { get; set; }
        public int Id { get; set; } = 0;
        public string SearchText { get; set; } = string.Empty;
        public int UserID { get; set; } = 0;
        public StatusType StatusID { get; set; }
        public int paymentModes { get; set; }
    }
    public class ProductSearchItem: SearchItem
    {
        public int CategoryID { get; set; } = 0;
    }
    public class UpdateAdminApprovelStatus: SearchItem
    {
        public string Remark { get; set; }
        public StatusType StatusID { get; set; }
    }
}
