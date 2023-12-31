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
    public class WishListSlide : WishList
    {
        public int WishListId { get; set; }
        public decimal MRP { get; set; }
        public decimal SellingCost { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
    }
    public class CartItemSlide : CartItem
    {
        public int CartItemId { get; set; }
        public decimal MRP { get; set; }
        public decimal SellingCost { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Attributes { get; set; }
        public bool IsCod { get; set; }
        public decimal SpecialDiscount { get; set; }
        public string Discounttype { get; set; }
        public string CouponCode { get; set; }
        public decimal CouponDiscountAmount { get; set; }
        public bool IsFixed { get; set; }
        public bool IsCouponApplied { get; set; }
        public int AvailableQuantity { get; set; }
        public decimal UserWalletAmount { get; set; }
    }
    public class CartWishlistCount
    {
        public int Items { get; set; }
        public string Type { get; set; }
    }
    public class CartItemsTotalVM
    {
        public List<CartItemSlide> CartItemSlides { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalMRP { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal SpecialDiscount { get; set; }
        public IEnumerable<Coupon> Coupons { get; set; }
    }
}
