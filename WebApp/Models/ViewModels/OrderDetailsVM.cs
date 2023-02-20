using Entities.Enums;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebApp.Models.ViewModels
{
    public class OrderDetailsVM
    {
        public int ID { get; set; }
        public string Remark { get; set; }
        public StatusType StatusID { get; set; }
        public string InvoiceNumber { get; set; }
        public string ImagePaths { get; set; }
        public List<IFormFile> Files { get; set; }
    }
    public class TrackingModel
    {
        public string TrackID { get; set; }
        public string URL { get; set; }
    }
    public class ReplaceOrderImageVM
    {
        public List<IFormFile> Files { get; set; }
        public int OrderId { get; set; }
    }
}
