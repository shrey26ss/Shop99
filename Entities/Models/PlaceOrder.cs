using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class PlaceOrderReq
    {
        public int UserID { get; set; }
        public int AddressID { get; set; }
        public int PaymentMode { get; set; }
        public string Remark { get; set; }
    }
    public class PlaceOrderResponse
    {
        public string OrderID { get; set; }
        public int UserID { get; set; }
        public int AddressTypeID { get; set; }
        public int UserAddressID { get; set; }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public int Pincode { get; set; }
        public string HouseNo { get; set; }
        public string Area { get; set; }
        public string Landmark { get; set; }
        public string TownCity { get; set; }
        public ResponseStatus StatusCode { get; set; }
        public string ResponseText { get; set; }
    }

}
