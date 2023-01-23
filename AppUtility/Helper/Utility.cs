using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AppUtility.Extensions;
using System.Drawing.Imaging;
using System.Drawing;   
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using OfficeOpenXml;
using Response = AppUtility.UtilityModels;

namespace AppUtility.Helper
{
    public class Utility
    {
        public static Utility O => instance.Value;
        private static Lazy<Utility> instance = new Lazy<Utility>(() => new Utility());
        private Utility() { }
        public string GetErrorDescription(int errorCode)
        {
            string error = ((Errorcodes)errorCode).DescriptionAttribute();
            return error;
        }
        public Response UploadFile(FileUploadModel request)
        {
            var response = Validate.O.IsFileValid(request.file);
            if (response.StatusCode == ResponseStatus.Success)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(request.FilePath);
                    if (!Directory.Exists(sb.ToString()))
                    {
                        Directory.CreateDirectory(sb.ToString());
                    }
                    var filename = ContentDispositionHeaderValue.Parse(request.file.ContentDisposition).FileName.Trim('"');
                    string originalExt = Path.GetExtension(filename).ToLower();
                    string[] Extensions = { ".png", ".jpeg", ".jpg", ".svg" };
                    if (Extensions.Contains(originalExt))
                    {
                        //originalExt = ".jpg";
                    }
                    //string originalFileName = Path.GetFileNameWithoutExtension(filename).ToLower() + originalExt;
                    if (string.IsNullOrEmpty(request.FileName))
                    {
                        request.FileName = filename;//Path.GetFileNameWithoutExtension(request.FileName).ToLower() + originalExt;
                    }
                    //request.FileName = string.IsNullOrEmpty(request.FileName) ? originalFileName.Trim() : request.FileName;
                    sb.Append(request.FileName);
                    using (FileStream fs = File.Create(sb.ToString()))
                    {
                        request.file.CopyTo(fs);
                        fs.Flush();
                        if (request.IsThumbnailRequired)
                        {
                            GenrateThumbnail(request.file, request.FileName, 20L);
                        }
                    }
                    response.StatusCode = ResponseStatus.Success;
                    response.ResponseText = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    response.ResponseText = "Error in file uploading. Try after sometime...";
                }
            }
            return response;
        }
        public bool GenrateThumbnail(IFormFile file, string fileName, long quality = 20L)
        {
            string tempImgNameWithPath = string.Concat(FileDirectories.Thumbnail, fileName);
            var newimg = new Bitmap(file.OpenReadStream());
            ImageCodecInfo jgpEncoder = GetEncoderInfo("image/jpeg");
            // for the Quality parameter category.
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);
            myEncoderParameters.Param[0] = myEncoderParameter;
            try
            {
                if (File.Exists(tempImgNameWithPath))
                {
                    File.Delete(tempImgNameWithPath);
                }
                newimg.Save(tempImgNameWithPath, jgpEncoder, myEncoderParameters);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }

        public string GetRole(int roleId)
        {
            string error = ((Role)roleId).ToString();
            return error;
        }

        public Dictionary<string, dynamic> ConvertToDynamicDictionary(object someObject)
        {
            var res = someObject.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => (dynamic)prop.GetValue(someObject, null));
            return res;
        }

        public Dictionary<string, string> ConvertToDictionary(object someObject)
        {
            var res = someObject.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => (string)prop.GetValue(someObject, null));
            return res;
        }

        public string GenrateRandom(int length, bool isNumeric = false)
        {
            string valid = "abcdefghjkmnpqrstuvwxyzABCDEFGHJKMNPQRSTUVWXYZ23456789";
            if (isNumeric)
            {
                valid = "1234567890";
            }
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public byte[] ConvertBitmapToBytes(Bitmap image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }
        public ExcelResponse<byte[]> ExportToExcel(DataTable dataTable, string[] removableCol = null)
        {
            ExcelResponse<byte[]> response = new ExcelResponse<byte[]>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Something went wrong"
            };
            try
            {
                if (removableCol != null)
                {
                    foreach (string str in removableCol)
                    {
                        if (dataTable.Columns.Contains(str))
                        {
                            dataTable.Columns.Remove(str);
                        }
                    }
                }

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("sheet1");
                    worksheet.Cells["A1"].LoadFromDataTable(dataTable, PrintHeaders: true);
                    worksheet.Row(1).Height = 20;
                    worksheet.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Row(1).Style.Font.Bold = true;
                    for (var col = 1; col < dataTable.Columns.Count + 1; col++)
                    {
                        worksheet.Column(col).AutoFit();
                    }
                    var exportToExcel = new InMemoryFile
                    {
                        Content = package.GetAsByteArray()
                    };
                    response.data = exportToExcel.Content;
                    response.StatusCode = ResponseStatus.Success;
                    response.ResponseText = ResponseStatus.Success.ToString();
                }
            }
            catch (Exception ex)
            {
                response.ResponseText = ex.Message;
            }
            return response;
        }
    }
}
