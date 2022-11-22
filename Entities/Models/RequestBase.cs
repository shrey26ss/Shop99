
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
    }
    public class SearchItem
    {
        public int Id { get; set; } = 0;
    }
}
