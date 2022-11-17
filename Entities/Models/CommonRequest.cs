using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class CommonRequest<T> where T : class
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
