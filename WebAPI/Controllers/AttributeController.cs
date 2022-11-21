using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class AttributeController : Controller
    {
        private readonly IAttributes _attr;

        public AttributeController(IAttributes attr)
        {
            _attr = attr;            
        }

        [Route("Attribute/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdateBrand(RequestBase<Attributes> request)
        {
            return Ok(await _attr.AddUpdate(request));
        }

        [Route("Attribute/GetAttributes")]
        public async Task<IActionResult> GetAttributes(RequestBase<SearchItem> request)
        {
            return Ok(await _attr.GetAttributes(request));
        }
    }
}
