using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class States : StateDDL
    {
        public int CountryId { get; set; }
        public bool IsActive { get; set; }
    }
    public class StateDDL
    {
        public int Id { get; set; }
        public string StateName { get; set; }
    }
}
