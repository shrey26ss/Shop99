using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("/api/")]
    public class CountryController : ControllerBase
    {
        public readonly ICountry _country;
        public CountryController(ICountry country) => _country = country;
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("Country/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdate(Country req) => Ok(await _country.AddUpdate(req));
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("Country/ChangeStatus")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeStatus(SearchItem req) => Ok(await _country.ChangeStatus(req));
        [Route("Country/GetCountries")]
        public async Task<IActionResult> GetCountries() => Ok(await _country.GetList());
    }
}
