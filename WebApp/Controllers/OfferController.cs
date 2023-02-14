using AppUtility.APIRequest;
using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.API;
using System.Net;
using System.Threading.Tasks;
using WebApp.Models;
using System.Collections.Generic;
using System.Linq;
using WebApp.Middleware;
using System;
using Entities.Enums;
using AppUtility.Helper;
using WebApp.Models.ViewModels;
using Service.Models;
using WebApp.AppCode.Attributes;
namespace WebApp.Controllers
{
    [Authorize]
    public class OfferController : Controller
    {
        private string _apiBaseURL;
        private IDDLHelper _ddl;

        public OfferController(AppSettings appSettings, IDDLHelper ddl) //IRepository<EmailConfig> emailConfig, 
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _ddl = ddl;
        }
        [HttpGet("/Offers")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("Offer/OffersList")]
        public async Task<ActionResult> OffersList(int Id)
        {
            List<GetOffers> Offers = await GetOffers(Id);
            return PartialView(Offers);
        }
        private async Task<List<GetOffers>> GetOffers(int Id)
        {
            List<GetOffers> offers = new List<GetOffers>();
            string _token = User.GetLoggedInUserToken();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Offers/GetOffers", JsonConvert.SerializeObject(new SearchItem { Id = Id }), _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<GetOffers>>>(apiResponse.Result);
                offers = deserializeObject.Result;
            }
            return offers;
        }
        [Authorize(Roles = "1")]
        // GET: CategoryController/Create
        public async Task<IActionResult> OfferCreate(int Id = 0)
        {


            var offer = new OfferViewModel();
            if (Id != 0)
            {
                string _token = User.GetLoggedInUserToken();
                var offerRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Offers/GetOffers", JsonConvert.SerializeObject(new SearchItem { Id = Id }), _token);
                if (offerRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<List<OfferViewModel>>>(offerRes.Result);
                    offer = deserializeObject.Result.FirstOrDefault();
                }
            }
            offer.offerstypeDDLs = await _ddl.GetOfferDDL(GetToken(), _apiBaseURL);
            return PartialView(offer);
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        [ValidateAjax]
        public async Task<ActionResult> OfferCreate(GetOffers offers)
        {
            Response response = new Response();
            try
            {
                var categoryrRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Offers/OfferAddUpdate", JsonConvert.SerializeObject(offers), User.GetLoggedInUserToken());
                if (categoryrRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    response = JsonConvert.DeserializeObject<Response>(categoryrRes.Result);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(response);
        }
        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }
        [Authorize(Roles = "1")]
        public async Task<IActionResult> UpdateIsActiveOffer(OfferUpdateIsActive req)
        {
            var res = new Response();
            if (req.OfferID >= 1)
            {
                string _token = User.GetLoggedInUserToken();
                var categoryrRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Offers/UpdateIsActiveOffer", JsonConvert.SerializeObject(req), _token);
                if (categoryrRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    res = JsonConvert.DeserializeObject<Response>(categoryrRes.Result);
                }
            }
            return Json(res);
        }
    }
}
