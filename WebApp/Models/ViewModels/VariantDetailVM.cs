using System.Collections.Generic;
using Entities.Models;
namespace WebApp.Models.ViewModels
{
    public class VariantDetailVM
    {
        public IEnumerable<ProductAttributes> Attributes { get; set; }
    }
}
