using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Thandizo.ApiExtensions.Filters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = string.Join("; ", context.ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                context.Result = new BadRequestObjectResult(errors);
            }
        }
    }
}
