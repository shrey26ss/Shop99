using Entities.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class PictureInformationReq
    {
        public string Color { get; set; }
        public int GroupId { get; set; }
        public int DisplayOrder { get; set; }
        public string Title { get; set; }
        public string Alt { get; set; }
        public IFormFile file { get; set; }
    }
}
