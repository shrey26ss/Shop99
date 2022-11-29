using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;

namespace AppUtility.Helper
{
    public class ImageResizer
    {
        private readonly long allowedFileSizeInByte;
        private readonly byte[] fileInBytes;
        private readonly string fileExtention;
        public ImageResizer(long allowedSize, byte[] fileInBytes, string fileExtention)
        {
            this.allowedFileSizeInByte = allowedSize;
            this.fileInBytes = fileInBytes;
            this.fileExtention = fileExtention;
        }

        public ImageResizer()
        {

        }
        public Bitmap ScaleImage(Bitmap image, double scale)
        {
            int newWidth = (int)(image.Width * scale);
            int newHeight = (int)(image.Height * scale);

            Bitmap result = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                g.DrawImage(image, 0, 0, result.Width, result.Height);
            }
            return result;
        }

        public byte[] ScaleImage()
        {
            byte[] returnBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(fileInBytes, 0, fileInBytes.Length);
                Bitmap bmp = (Bitmap)Image.FromStream(ms);
                SaveTemporary(bmp, ms, 100);
                while (ms.Length > allowedFileSizeInByte)
                {
                    double scale = Math.Sqrt
                    ((double)allowedFileSizeInByte / (double)ms.Length);
                    ms.SetLength(0);
                    bmp = ScaleImage(bmp, scale);
                    SaveTemporary(bmp, ms, 100);
                }
                if (bmp != null)
                    bmp.Dispose();
                returnBytes= ms.ToArray();
            }
            return returnBytes;
        }

        public Bitmap ResizeImage(IFormFile file, int width,int height)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                Bitmap image = (Bitmap)Image.FromStream(ms);
                Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using (Graphics g = Graphics.FromImage(result))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    g.DrawImage(image, 0, 0, result.Width, result.Height);
                }
                return result;
            }
            return null;
        }

        private void SaveTemporary(Bitmap bmp, MemoryStream ms, int quality)
        {
            EncoderParameter qualityParam = new EncoderParameter
                (System.Drawing.Imaging.Encoder.Quality, quality);
            var codec = GetImageCodecInfo();
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            bmp.Save(ms, codec, encoderParams);
        }

        private ImageCodecInfo GetImageCodecInfo()
        {
            switch (fileExtention)
            {
                case ".bmp": return ImageCodecInfo.GetImageEncoders()[0];
                case ".jpg":
                case ".jpeg": return ImageCodecInfo.GetImageEncoders()[1];
                case ".gif": return ImageCodecInfo.GetImageEncoders()[2];
                case ".tiff": return ImageCodecInfo.GetImageEncoders()[3];
                case ".png": return ImageCodecInfo.GetImageEncoders()[4];
                default: return null;
            }
        }
    }
}
