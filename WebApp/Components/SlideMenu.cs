using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Middleware;
using WebApp.Servcie;
using System.Security.Claims;
using System;
using Entities.Enums;

namespace WebApp.Components
{
    public class SlideMenu : ViewComponent
    {

        public SlideMenu()
        {

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var identity = User.Identity as ClaimsIdentity;
            UserInfo ui = new UserInfo();
            try
            {
                foreach (var claim in identity.Claims)
                {
                    if (claim.Type.Contains("Id"))
                    {
                        ui.Id = Convert.ToInt16(claim.Value);
                    }
                    else if (claim.Type.Contains("role"))
                    {
                        ui.role = (Role)Enum.Parse(typeof(Role), claim.Value); ;
                    }
                    else if (claim.Type.Contains("name"))
                    {
                        ui.name = claim.Value;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return await Task.FromResult((IViewComponentResult)View("slidemenu", ui));
        }
    }
}
