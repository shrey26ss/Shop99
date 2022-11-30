using Entities.Models;
using System.Collections.Generic;

namespace WebApp.Models.ViewModels
{
    public class CategoryViewModel : Category
    {
        public List<CategoryDDL> categoryDDLs { get; set; }
    }
}
