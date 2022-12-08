using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Entities.Models
{
    public class Filters 
    {
        public int AttributeId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public string AttributeValue { get; set; }
        public IEnumerable<FiltersAttributes> attributes { get; set; }
    }
    public class FiltersAttributes
    {
        public string AttributeValue { get; set; }
    }
}
