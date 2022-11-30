using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Servcie;

namespace WebApp.Components
{
    public class TopMenu : ViewComponent
    {
        private readonly ICategory _category;
        public TopMenu(ICategory category)
        {
            _category = category;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Request request = new Request
            {
                //LoginId= User.GetLoggedInUserId<int>()
            };
            var model = _category.GetMenu().Result;
            return await Task.FromResult((IViewComponentResult)View("topmenu", model));
        }
    }
}
