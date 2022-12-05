using Entities.Models;
using System.Collections.Generic;

namespace WebApp.Models.ViewModels
{
    public class CatAttrMappingViewModel
    {
        public List<CategoryDDL> Categories { get; set; }
        public List<AtttributeMapping> MappedList { get; set; }
        public List<AtttributeMapping> UnMappedList { get; set; }
    }
}
