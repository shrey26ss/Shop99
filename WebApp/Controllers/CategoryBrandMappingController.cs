using AppUtility.APIRequest;
using AppUtility.Helper;
using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.Middleware;
using WebApp.Models;
using WebApp.Models.ViewModels;

namespace WebApp.Controllers
{
    [Authorize(Roles ="1")]
    public class CategoryBrandMappingController : Controller
    {
        private string _apiBaseURL;
        private IDDLHelper _ddl;
        public CategoryBrandMappingController(ILogger<AccountController> logger, IMapper mapper, AppSettings appSettings, IDDLHelper ddl)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _ddl = ddl;
        }
        [Route("CategoryBrandMapping/Index")]
        [Route("Index")]
        public async Task<IActionResult> Index(int Id = 0)
        {
            var model = new CatBrandMappingViewModel { CategoryId = Id };
            model.Categories = await _ddl.GetCategoryDDL(GetToken(), _apiBaseURL);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> GetMappedItemList(int CategoryId)
        {
            var model = new CatBrandMappingViewModel();
            model.MappedList = await GetUnMappedandMapped(CategoryId, "GetMapped");
            model.UnMappedList = await GetUnMappedandMapped(CategoryId, "GetUnMapped");
            return PartialView("PartialView/_MappedItemList", model);
        }
        [HttpPost]
        public async Task<IActionResult> MapItems(CategoryBrandMapping model)
        {
            var response = new Response();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/CategoryBrandMapping/AddUpdate", JsonConvert.SerializeObject(model), GetToken());
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                response = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
            }
            return Json(response);
        }

        private async Task<List<BrandMapping>> GetUnMappedandMapped(int categoryId, string type)
        {
            var list = new List<BrandMapping>();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/CategoryBrandMapping/{type}", JsonConvert.SerializeObject(new SearchItem { Id = categoryId }), GetToken());
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var _ = JsonConvert.DeserializeObject<Response<IEnumerable<BrandMapping>>>(apiResponse.Result);
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
