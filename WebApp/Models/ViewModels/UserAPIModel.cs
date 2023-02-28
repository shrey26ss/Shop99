using Entities.Enums;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebApp.Models.ViewModels
{
    public class UserAPIModel
    {
        public List<IFormFile> Files { get; set; }
        public int UserID { get; set; }
    }
    public class Profileviewmodel
    {
        public Role Role { get; set; }
        public int loginId { get; set; }
        public string profilepic { get; set; }
    }
}
