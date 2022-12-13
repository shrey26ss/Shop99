using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{    
    [ApiController]
    [Route("/api/")]
    public class StateController : ControllerBase
    {
        public readonly IStates _states;
        public StateController(IStates states) => _states = states;
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("State/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdate(States req) => Ok(await _states.AddUpdate(req));
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("State/ChangeStatus")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeStatus(SearchItem req) => Ok(await _states.ChangeStatus(req));
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("State/GetSates")]
        public async Task<IActionResult> GetSates() => Ok(await _states.GetList());
        [AllowAnonymous]
        [Route("State/GetStateDDL")]
        public async Task<IActionResult> GetStateDDL() => Ok(await _states.GetDDL());
    }
}
