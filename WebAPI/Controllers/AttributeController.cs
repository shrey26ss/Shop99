﻿using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class AttributeController : ControllerBase
    {
        private readonly IAttributes _attr;

        public AttributeController(IAttributes attr)
        {
            _attr = attr;            
        }

        [Route("Attribute/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdateBrand(Attributes req)
        {
            return Ok(await _attr.AddUpdate(new RequestBase<Attributes>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }

        [Route("Attribute/GetAttributes")]
        public async Task<IActionResult> GetAttributes(SearchItem req)
        {
            return Ok(await _attr.GetAttributes(new RequestBase<SearchItem>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [Route("Attribute/GetAttributeDDL")]
        public async Task<IActionResult> GetAttributeDDL()
        {
            return Ok(await _attr.GetAttributeDDL());
        }
    }
}
