using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class Cities : CityDDL
    {
        public int StateId { get; set; }
        public bool IsActive { get; set; }
    }
    public class CityDDL
    {
        public int Id { get; set; }
        public string CityName { get; set; }
    }
}
