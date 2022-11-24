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
        public static string Receipt = "wwwroot/receipt/";
        public static string Thumbnail = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Thumbnail/");
        public static string ProfilePic = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Profile/");
        public static string JsonDoc = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/doc/");
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
}
