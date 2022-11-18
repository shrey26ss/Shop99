using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class RequestBase<T> : Request
    {
        public T Data { get; set; } 
    }
    public class Request
    {
        public int LoginId { get; set; }
        public int RoleId { get; set; }
    }
    public class SearchItem
    {
        public int Id { get; set; } = 0;
    }
}
