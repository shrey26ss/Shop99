using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Linq;

namespace WebApp.AppCode.Attributes
{
    public class ValidateAjaxAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;

            if (!modelState.IsValid)
            {

                var result = modelState.Keys
                    .Where(x => modelState[x].Errors.Count > 0).Select(y => new
                    {
                        Key = y,
                        Errors = modelState[y].Errors.Select(y => y.ErrorMessage).ToArray()
                    });
                context.Result = new JsonResult(result);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}
