using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class WebsiteinfoModel
    {
        public int Id { get; set; }
        public string Whitelogo { get; set; }
        public string Coloredlogo { get; set; }
        public string Companyname { get; set; }
        public string Companydomain { get; set; }
        public string CompanyemailID { get; set; }
        public string Companymobile { get; set; }
        public string Companyaddress { get; set; }
        public string Footerdescreption { get; set; }
        public int LoginID { get; set; }
    }
}
