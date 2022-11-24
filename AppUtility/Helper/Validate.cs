using AppUtility.Extensions;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace AppUtility.Helper
{
    public class Validate
    {
        public static Validate O => instance.Value;
        private static Lazy<Validate> instance = new Lazy<Validate>(() => new Validate());
        private Validate() { }
        public string WebURLExpression = @"^(http|https)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
        public string ReplaceAllSpecials(string s) => Regex.Replace(s, "[^0-9A-Za-z]+", " ");
        public bool IsWhiteSpace(string s) => Regex.IsMatch(s, @"(\S)+");
        public bool IsNumeric(string s) => Regex.IsMatch(s, @"^[0-9]+$") && s != "";
        public bool IsDecimal(object s) => Regex.IsMatch(Convert.ToString(s), @"^[0-9]+(\.?[0-9]?)") && !string.IsNullOrEmpty(Convert.ToString(s));
        public bool IsAlphaNumeric(string s) => Regex.IsMatch(s, @"^[a-zA-Z0-9]*$") && s != "";
        public bool IsMobile(string s) => Regex.IsMatch(s, @"^([6-9]{1})([0-9]{9})$");
        public bool IsStartsWithNumber(string s) => Regex.IsMatch(s, @"^\d+");
        public bool IsEndWithNumber(string s) => Regex.IsMatch(s, @"(\d+)$");
        public bool IsEmail(string s) => Regex.IsMatch(s, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
        public bool IsPAN(string s) => Regex.IsMatch(s, @"^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$");
        public bool IsGSTIN(string s) => Regex.IsMatch(s, @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$");
        public bool IsAADHAR(string s) => Regex.IsMatch(s, @"^\d{4}\d{4}\d{4}$");
        public bool IsPANUpper(string s) => Regex.IsMatch(s, @"^([A-Z]){5}([0-9]){4}([A-Z]){1}?$");
        public bool IsPinCode(string s) => Regex.IsMatch(s, @"^\d{6}$");
        public bool IsValidBankAccountNo(string s) => (s.Length >= 9 && s.Length <= 18);
        //public bool IsWebURL(string s) => Regex.IsMatch(s, @"^http(s)?://([\\w-]+.)+[\\w-]+(/[\\w- ./?%&=])?$");
        public bool IsWebURL(string s) => Regex.IsMatch(s, WebURLExpression);

        public bool IsDateIn_dd_MMM_yyyy_Format(string s, char seprator = ' ')
        {
            if (string.IsNullOrEmpty(s))
                return false;
            if (!s.Contains(seprator + ""))
                return false;
            var sArr = s.Split(seprator);
            if (sArr.Length != 3)
                return false;
            if (sArr[0].Length != 2 || !IsNumeric(sArr[0]))
                return false;
            if (Convert.ToInt32(sArr[0]) > 31 || Convert.ToInt32(sArr[0]) < 0)
                return false;
            if (sArr[1].Length != 3 || IsNumeric(sArr[1]) || !Months.Contains(sArr[1]))
                return false;
            if (sArr[2].Length != 4 || !IsNumeric(sArr[2]))
                return false;

            return true;
        }

        public string[] Months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        private string[] LowerMonth => Months.Select(s => s.ToLower()).ToArray();
        public bool IsFileExecutable(byte[] FileContent)
        {
            if (FileContent == null)
                return false;
            byte[] newByteArr = GetSubBytes(FileContent, 0, 2);
            return Encoding.UTF8.GetString(newByteArr).ToUpper() == "MZ" || Encoding.UTF8.GetString(newByteArr).ToUpper() == "ZM";
        }
        private byte[] GetSubBytes(byte[] oldBytes, int start, int len)
        {
            if (oldBytes.Length >= len && start > -1 && start < len)
            {
                byte[] newByteArr = new byte[len];
                Array.Copy(oldBytes, start, destinationArray: newByteArr, destinationIndex: 0, length: len);
                return newByteArr;
            }
            return null;
        }
        public readonly IEnumerable<string> FileFormatsAllowed = new List<string> { ".WEBP", ".JPEG", ".JPG", ".PNG", ".DOCX", ".GIF", ".PDF", ".ZIP", ".RAR" };
        private IEnumerable<string> CheckFFSignature = new List<string> { "RIFF", "EXIF", "JPG", "JPEG", "JFIF", "PNG", "GIF", "%PDF", "PK", "GIF" };
        public string MaskNumeric(string s, string replacewith) => Regex.Replace(s, "[0-9]", replacewith);
        public string Prefix(string s, int PLen = 2)
        {
            return !string.IsNullOrEmpty(s) && !IsNumeric(s ?? "") ? s.Substring(0, PLen) : "";
        }

        public string LoginID(string s, int PLen = 2) => !string.IsNullOrEmpty(s) ? s.Substring(PLen, s.Length - PLen) : "0";

        public bool ValidateJSON(string s)
        {
            try
            {
                JToken.Parse(s);
                return true;
            }
            catch { }
            return false;
        }


        public const int ImageMinimumBytes = 512;

        public bool IsValidImage(IFormFile postedFile)
        {

            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (!string.Equals(postedFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(postedFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(postedFile.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(postedFile.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(postedFile.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(postedFile.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            var postedFileExtension = Path.GetExtension(postedFile.FileName);
            if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            MemoryStream postedFileMs = new MemoryStream();
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    postedFile.CopyTo(ms);
                    if (!ms.CanRead)
                    {
                        return false;
                    }
                    //------------------------------------------
                    //   Check whether the image size exceeding the limit or not
                    //------------------------------------------ 
                    if (ms.Length < ImageMinimumBytes)
                    {
                        return false;
                    }

                    byte[] buffer = new byte[ImageMinimumBytes];
                    ms.Read(buffer, 0, ImageMinimumBytes);
                    string content = Encoding.UTF8.GetString(buffer);
                    if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                    {
                        return false;
                    }
                    postedFileMs = ms;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                postedFileMs.Position = 0;
            }
            return true;
        }

        public bool IsValidPDF(IFormFile postedFile)
        {
            //-------------------------------------------
            //  Check the file extension
            //-------------------------------------------
            var postedFileExtension = Path.GetExtension(postedFile.FileName);
            if (!string.Equals(postedFileExtension, ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            //-------------------------------------------
            //  Check the file mime types
            //-------------------------------------------
            if (!string.Equals(postedFile.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    postedFile.CopyTo(ms);
                    if (!ms.CanRead)
                    {
                        return false;
                    }
                    //------------------------------------------
                    //   Check whether the image size exceeding the limit or not
                    //------------------------------------------ 
                    if (ms.Length < ImageMinimumBytes)
                    {
                        return false;
                    }

                    byte[] buffer = new byte[ImageMinimumBytes];
                    ms.Read(buffer, 0, ImageMinimumBytes);
                    string content = Encoding.UTF8.GetString(buffer);
                    if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool IsValidEmployeeCount(int EmployeeCount, string Scope)
        {
            bool result = true;
            Scope = Scope.Replace("and", ",");
            var Arr = Scope.Split(',');
            if (EmployeeCount < Arr.Count())
            {
                result = false;
            }
            return result;
        }


        public bool IsFileAllowed(byte[] fileContent, string ext)
        {
            if (fileContent == null)
                return false;
            if (string.IsNullOrEmpty(ext))
                return false;
            if (!ext.ToUpper().In(FileFormatsAllowed))
                return false;
            var SubByte = GetSubBytes(fileContent, 0, 20);
            string Start20BytesStr = SubByte?.Length > 0 ? Encoding.UTF8.GetString(SubByte) : "";
            if (Start20BytesStr.Length < 1)
                return false;
            if (!CheckFFSignature.Any(Start20BytesStr.ToUpper().Contains))
                return false;
            return true;
        }

        public Response IsFileValid(IFormFile file)
        {
            var res = new Response
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Temperory Error"
            };
            if (file != null)
            {
                var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string ext = Path.GetExtension(filename).ToLower();

                byte[] filecontent = null;
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    filecontent = ms.ToArray();
                }
                if (!IsFileAllowed(filecontent, ext))
                {
                    res.ResponseText = "Invalid File Format!";
                    return res;
                }
                else if (!file.ContentType.Any())
                    res.ResponseText = "File not found!";
                else if (file.Length < 1)
                    res.ResponseText = "Empty file not allowed!";
                else if (file.Length / 1024 > 1024 && !ext.ToLower().In(".zip", ".rar"))
                    res.ResponseText = "File size exceeded! Not more than 1 MB is allowed";
                else
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = "it is a valid file";
                }

            }
            else
            {
                res.ResponseText = "No File Found";
            }
            return res;
        }


    }
}
