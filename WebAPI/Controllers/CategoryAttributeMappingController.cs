using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "1,3,4")]
    [ApiController]
    [Route("/api/")]
    public class CategoryAttributeMappingController : ControllerBase
    {
        private readonly ICategoryAttributeMapping _mapping;

        public CategoryAttributeMappingController(ICategoryAttributeMapping mapping) => _mapping = mapping;
        [Route("CategoryAttributeMapping/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdate(CategoryAttrMapping req) => Ok(await _mapping.AddUpdate(new RequestBase<CategoryAttrMapping>
        {
            Data = req,
            LoginId = User.GetLoggedInUserId<int>()
        }));

        [Route("CategoryAttributeMapping/GetMapped")]
        public async Task<IActionResult> GetMapped(SearchItem req) => Ok(await _mapping.GetMapped(new RequestBase<SearchItem>
        {
            Data = req
        }));

        [Route("CategoryAttributeMapping/GetUnMapped")]
        public async Task<IActionResult> GetUnMapped(SearchItem req) => Ok(await _mapping.GetUnMapped(new RequestBase<SearchItem>
        {
            Data = req
        }));
    }
}
