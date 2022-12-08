using Entities.Models;
using System.Collections.Generic;

namespace WebApp.Models.ViewModels
{
    public class CatAttrMappingViewModel
    {
        public int CategoryId { get; set; }
        public List<CategoryDDL> Categories { get; set; }
        public List<AtttributeMapping> MappedList { get; set; }
        public List<AtttributeMapping> UnMappedList { get; set; }
    }
}
