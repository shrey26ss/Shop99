using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebApp.AppCode;
using WebApp.AppCode.Helper;
using WebApp.Middleware;

namespace WebApp.Controllers
{
    [Authorize(Roles ="4")]
    public class DeveloperController : Controller
    {
        private readonly IGenericMethods _convert;
        public DeveloperController(IGenericMethods convert)
        {
            _convert = convert;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ClearImage()
        {
            DeleteUnutilizedImage();
            return RedirectToAction("Index", "Developer");
        }
        private async Task DeleteUnutilizedImage()
        {
            DirectoryInfo d = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Image/Product"));
            FileInfo[] Files = d.GetFiles("*.jpeg", SearchOption.AllDirectories);
            var filesFromFolder = Files.ToList();
            var fileFromTable = await _convert.GetList<PictureSubInfo>("Developer/GetList",User.GetLoggedInUserToken());
            List<string> filenamesFromTable = fileFromTable.ToList().Select(s => (string)s.Path.Split("/").Last()).ToList();
            List<string> optionList = new List<string>();
            for(int i = 0; i < Files.Length; i++)
            {
                if (!filenamesFromTable.Contains(Files[i].Name))
                {
                    string filePath = "";
                    if(Files[i].FullName.Contains("400_400"))
                        filePath = d + "/400_400/" + Files[i].Name;
                    else if(Files[i].FullName.Contains("700_700"))
                        filePath = d + "/700_700/" + Files[i].Name;
                    else
                        filePath = d + "/" + Files[i].Name;
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    
                }
            }
        }
    }
}
