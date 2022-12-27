using AppUtility.APIRequest;
using AppUtility.Helper;
using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WebApp.Middleware;
using WebApp.Models;
using WebApp.Models.ViewModels;
using System.Linq;

namespace WebApp.Controllers
{
    [Authorize]
    public class CategoryAttributeMappingController : Controller
    {
        private string _apiBaseURL;
        private IDDLHelper _ddl;
        public CategoryAttributeMappingController(ILogger<AccountController> logger, IMapper mapper, AppSettings appSettings,IDDLHelper ddl)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _ddl = ddl;
        }
        [Route("CategoryAttributeMapping/Index")]
        [Route("Index")]
        public async Task<IActionResult> Index(int Id = 0)
        {
            var model = new CatAttrMappingViewModel { CategoryId = Id };
            model.Categories = await _ddl.GetCategoryDDL(GetToken(), _apiBaseURL);
            return View(model);
        }

        
        //[Route("_CategoryAttribute")]
        public async Task<IActionResult> _CategoryAttribute(int Id = 0)
        {
            var model = new CatAttrMappingViewModel { CategoryId = Id };
            model.Categories = await _ddl.GetCategoryDDL(GetToken(), _apiBaseURL);
            return PartialView("PartialView/_CategoryAttribute", model);
        }
        [HttpPost]
        public async Task<IActionResult> GetMappedItemList(int CategoryId)
        {
            var model = new CatAttrMappingViewModel();
            model.MappedList = await GetUnMappedandMapped(CategoryId, "GetMapped");
            model.UnMappedList = await GetUnMappedandMapped(CategoryId, "GetUnMapped");
            return PartialView("PartialView/_MappedItemList", model);
        }
        [HttpPost]
        public async Task<IActionResult> MapItems(CategoryAttrMapping model)
        {
            var response = new Response();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/CategoryAttributeMapping/AddUpdate", JsonConvert.SerializeObject(model), GetToken());
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                response = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
            }
            return Ok(response);
        }

        private async Task<List<AtttributeMapping>> GetUnMappedandMapped(int categoryId, string type)
        {
            var list = new List<AtttributeMapping>();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/CategoryAttributeMapping/{type}", JsonConvert.SerializeObject(new SearchItem { Id = categoryId }), GetToken());
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var _ = JsonConvert.DeserializeObject<Response<IEnumerable<AtttributeMapping>>>(apiResponse.Result);
                list = _.Result.ToList();
            }
            return list;
        }

        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }
    }
}
