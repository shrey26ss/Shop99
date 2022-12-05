using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class CategoryAttrMapping : AtttributeMapping
    {
        public int CategoryId { get; set; } 
        public string CategoryName { get; set; } 
    }
    public class AtttributeMapping
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
    }
}
