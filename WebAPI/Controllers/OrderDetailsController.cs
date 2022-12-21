using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailsService _orderRepo;

        public OrderDetailsController(IOrderDetailsService orderRepo) => _orderRepo = orderRepo;
        [Route("OrderDetails/GetDetails")]
        public async Task<IActionResult> GetDetails(OrderDetailsRow req) => Ok(await _orderRepo.GetAsync(User.GetLoggedInUserId<int>()));
        [Route("OrderDetails/ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(OrderDetailsRow req) => Ok(await _orderRepo.ChengeStatusAsync(req));
    }
}
