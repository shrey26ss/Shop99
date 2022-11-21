using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class MenuItem : Category
    {
        public IEnumerable<MenuItem> Children { get; set; } = new List<MenuItem>();
    }
    public class Menu
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? ParentId { get; set; }
        public bool IsPublish { get; set; }
        public string Icon { get; set; }

        //public Menu(int id, string name, int? parentID)
        //{
        //    this.CategoryId = id;
        //    this.CategoryName = name;
        //    this.ParentId = parentID;
        //}
    }
}
