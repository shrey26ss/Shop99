using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Menu : Category
    {
        public IEnumerable<Menu> CategoryList { get; set; } = new List<Menu>();
    }
}
