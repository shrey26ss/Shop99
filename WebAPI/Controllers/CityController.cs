using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class CityController : ControllerBase
    {
        public readonly ICity _city;
        public CityController(ICity city) => _city = city;
        [Route("City/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdate(Cities req) => Ok(await _city.AddUpdate(req));
        [Route("City/ChangeStatus")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeStatus(SearchItem req) => Ok(await _city.ChangeStatus(req));
        [Route("City/GetCities")]
        public async Task<IActionResult> GetCities() => Ok(await _city.GetList());
        [AllowAnonymous]
        [Route("City/GetCityDDL")]
        public async Task<IActionResult> GetCityDDL() => Ok(await _city.GetDDL());
    }
}
