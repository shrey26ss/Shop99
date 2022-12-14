using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public bool IsActive { get; set; }
    }
}
