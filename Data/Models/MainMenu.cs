using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class MenuItem : Category
    {
        public IEnumerable<MenuItem> ChildNode { get; set; } = new List<MenuItem>();
    }
}
