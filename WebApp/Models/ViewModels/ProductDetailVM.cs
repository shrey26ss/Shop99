using System.Collections.Generic;

namespace WebApp.Models.ViewModels
{
    public class ProductDetailVM
    {

    }

    public class Filters
    {
        public int FilterId { get; set; }
        public string FilterName { get; set; }
        public IEnumerable<FilterValues> Values { get; set; }
    }

    public class FilterValues
    {
        public string Value { get; set; }
    }
}
