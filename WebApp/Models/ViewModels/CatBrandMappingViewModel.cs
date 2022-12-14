using Entities.Models;
using System.Collections.Generic;

namespace WebApp.Models.ViewModels
{
    public class CatBrandMappingViewModel
    {
        public int CategoryId { get; set; }
        public List<CategoryDDL> Categories { get; set; }
        public List<BrandMapping> MappedList { get; set; }
        public List<BrandMapping> UnMappedList { get; set; }
    }
}
