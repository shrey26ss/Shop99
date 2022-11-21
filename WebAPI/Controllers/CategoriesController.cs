using AutoMapper;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.API;
using Service.Identity;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _category;

        public CategoriesController(ICategoryService category)
        {
            _category= category;
        }

        [Route("Category/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdateCategory(RequestBase<Category> category)
        {
            return Ok(await _category.AddUpdate(category));
        }

        [Route("Category/GetCategory")]
        public async Task<IActionResult> GetCategory(RequestBase<SearchItem> request)
        {
            return Ok(await _category.GetCategories(request));
        }
        [Route("Category/GetMenu")]
        public async Task<IActionResult> GetMenu(Request request)
        {
            return Ok(await _category.GetMenu(request));
        }
    }
}
