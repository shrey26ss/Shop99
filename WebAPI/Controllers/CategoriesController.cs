﻿using AutoMapper;
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
using WebAPI.Middleware;

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
        public async Task<IActionResult> AddUpdateCategory(Category req)
        {
            return Ok(await _category.AddUpdate(new RequestBase<Category>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }

        [Route("Category/GetCategory")]
        public async Task<IActionResult> GetCategory(SearchItem req)
        {
            return Ok(await _category.GetCategories(new RequestBase<SearchItem>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [Route("Category/GetMenu")]
        public async Task<IActionResult> GetMenu()
        {
            Request request = new Request
            {
                LoginId= User.GetLoggedInUserId<int>()
            };
            return Ok(await _category.GetMenu(request));
        }
        [Route("Category/GetCategoriesDDL")]
        public async Task<IActionResult> GetCategoriesDDL()
        {
            return Ok(await _category.GetCategoriesDDL());
        }
    }
}
