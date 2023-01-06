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
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class CategoryBrandMappingController : ControllerBase
    {
        private readonly ICategoryBrandMapping _mapping;

        public CategoryBrandMappingController(ICategoryBrandMapping mapping) => _mapping = mapping;

        [Route("CategoryBrandMapping/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdate(CategoryBrandMapping req) => Ok(await _mapping.AddUpdate(new RequestBase<CategoryBrandMapping>
        {
            Data = req,LoginId = User.GetLoggedInUserId<int>()
        }));

        [Route("CategoryBrandMapping/GetMapped")]
        public async Task<IActionResult> GetMapped(SearchItem req) => Ok(await _mapping.GetMapped(new RequestBase<SearchItem>
        {
            Data = req
        }));

        [Route("CategoryBrandMapping/GetUnMapped")]
        public async Task<IActionResult> GetUnMapped(SearchItem req) => Ok(await _mapping.GetUnMapped(new RequestBase<SearchItem>
        {
            Data = req
        }));
    }
}
