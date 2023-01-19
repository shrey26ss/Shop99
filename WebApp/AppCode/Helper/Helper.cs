using Service.Models;
using System;
using System.IO;

namespace WebApp.AppCode.Helper
{
    public class Helper
    {
        public static Helper O { get { return Instance.Value; } }
        private static Lazy<Helper> Instance = new Lazy<Helper>(() => new Helper());
        private Helper() { }

        public Response DeleteFile(string path)
        {
            var response = new Response();
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
                response.StatusCode=Entities.Enums.ResponseStatus.Success;
                response.ResponseText="File deleted successfully";
            }
            else
            {
                response.ResponseText = "No Such File Exists";
            }
            return response;
        }
        public string GenerateInvoiceNumber(int Id)
        {
            string o = Id.ToString();
            string InvoiceNo = $"TID{o.PadLeft(7, '0')}A";
            return InvoiceNo;
        }
    }
}
