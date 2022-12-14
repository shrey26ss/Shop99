using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class PaymentMode
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool ISActive { get; set; }
    }
  
}
