using AppUtility.APIRequest;
using AutoMapper;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{

    [Authorize]
    public class AttributeController : Controller
    {
        private string _apiBaseURL;

        public AttributeController(ILogger<AccountController> logger, IMapper mapper, AppSettings appSettings) //IRepository<EmailConfig> emailConfig, 
        {

            _apiBaseURL = appSettings.WebAPIBaseUrl;
        }
        // GET: AttributeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AttributeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AttributeController/Create
        public async Task<ActionResult> Create(int Id)
        {
            Attributes attributes = new Attributes();
            if (Id != 0)
            {
                var attributesRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Attribute/GetAttributes", JsonConvert.SerializeObject(new SearchItem { Id = Id }));
                if (attributesRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<List<Attributes>>>(attributesRes.Result);
                    attributes = deserializeObject.Result.FirstOrDefault();
                }
            }
            return View(attributes);
        }

        // POST: AttributeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Attributes attributes)
        {
            Response response=new Response();
            try
            {
                var body = JsonConvert.SerializeObject(attributes);
                var categoryrRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Attribute/AddUpdate", body);
                if (categoryrRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response>(categoryrRes.Result);
                    response = deserializeObject;
                }
            }
            catch
            {
                return View();
            }
            return View(response);  
        }

        // GET: AttributeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AttributeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AttributeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AttributeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
