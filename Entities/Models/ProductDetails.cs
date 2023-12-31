﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class ProductDetails
    {
        public int VariantId { get; set; }
        public int ProductId { get; set; }
        public decimal MRP { get; set; }
        public decimal SellingCost { get; set; }
        public int Quantity { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string ProductName { get; set; }
        public string ProductTitle { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public int ReturnInDays { get; set; }
        public int Warranty { get; set; }
        public string WarrantyUnit { get; set; }
        public decimal Rating { get; set; }
        public int  TotalReviews { get; set; }
        public int WishListID { get; set; }
        public string Images { get; set; }
        public List<ProductAttributes> ProductAttributes { get; set; }
        public List<ProductPictureInfo> ProductPictureInfos { get; set; }
        public List<AttributeInfo> AttributeInfo { get; set; }
        public List<specialoffer> sepcialoffer { get; set; }
        public List<Coupon> Coupons { get; set; }
    }
    public class ProductAttributes
    {
        public int AttributeId { get; set; }
        public int ProductId { get; set; }
        public int VariantId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }
    public class ProductPictureInfo
    {
        public string Color { get; set; }
        public string Title { get; set; }
        public string Alt { get; set; }
        public string ImagePath { get; set; }
        public string ImgVariant { get; set; }
    }
}
