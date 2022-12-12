﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class CartWishList 
    {
       
    }
    public class WishList
    {
        public int VariantID { get; set; }
        public int EntryBy { get; set; }
    }
    public class CartItem
    {
        public int UserID { get; set; }
        public int VariantID { get; set; }
        public int Qty { get; set; }
    }
    public class CartItems : CartItem
    {
        public int CartId { get; set; }
        public decimal MRP { get; set; }
        public decimal SellingCost { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
    }

}
