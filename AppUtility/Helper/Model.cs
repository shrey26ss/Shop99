using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppUtility.Helper
{
    public class  FileContentType
    {
        public const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string PdfContentType = "application/x-pdf";
        public const string ZipContentType = "application/octet-stream";
    }
    public class FileDirectories
    {
        public const string CategorySuffix = "Image/Category/";
        public const string TopBannerSuffix = "Image/TopBanner/";
        public const string TopLowerBannerSuffix = "Image/TopLowerBanner/";
        public const string ProductSuffixDefault = "Image/Product/";
        public const string ProductRateSuffixDefault = "Image/RatingImages/{0}/";
        public const string ReplaceOrderImageSuffixDefault = "Image/ReplaceOrderImages/";
        public const string ProductSuffix = "Image/Product/{0}/";
        public const string WebsiteinfoSuffix = "Image/Website/";
        public static string Receipt = "wwwroot/receipt/";
        public static string Thumbnail = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Thumbnail/");
        public static string Category = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{CategorySuffix}");
        public static string ProfilePic = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Profile/");
        public static string ProductVariant = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{ProductSuffix}/");
        public static string ReplaceOrderImage = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{ReplaceOrderImageSuffixDefault}/");
        public static string JsonDoc = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/doc/");
        public static string TopBanner = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{TopBannerSuffix}");
        public static string Websiteinfo = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{WebsiteinfoSuffix}");
        public static string TopLowerBanner = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{TopLowerBannerSuffix}");
        public static string ProductRate = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{ProductRateSuffixDefault}");
    }

    public class FileUploadModel
    {
        public IFormFile file { get; set; }
        public List<IFormFile> Files { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int Id { get; set; }
        public bool IsThumbnailRequired { get; set; }
    }
    public class DOCType
    {
        public const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    }
}
