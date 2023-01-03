using System.Collections.Generic;
using Entities.Models;
namespace WebApp.Models.ViewModels
{
    public class VariantDetailVM
    {
        public IEnumerable<ProductAttributes> Attributes { get; set; }
        public VariantDetailsByAttributesResponse variantDetailsByAttributes { get; set; }
        public IEnumerable<PictureInformation> PictureInformation { get; set; }
    }
}
