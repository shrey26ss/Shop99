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
    public class StateController : ControllerBase
    {
        public readonly IStates _states;
      
        public StateController(IStates states) => _states = states;
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("State/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdate(States req) => Ok(await _states.AddUpdate(req));
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("State/ChangeStatus")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeStatus(SearchItem req) => Ok(await _states.ChangeStatus(req));
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("State/GetSates")]
        public async Task<IActionResult> GetSates() => Ok(await _states.GetList());
        [HttpPost]
        [AllowAnonymous]
        [Route("State/GetStateDDL")]
        public async Task<IActionResult> GetStateDDL() => Ok(await _states.GetDDL());
    }
}
