using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PTMS.Api.Config
{
    public class ValidatorActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                var message = new StringBuilder();

                foreach (var item in filterContext.ModelState)
                {
                    if (item.Value.Errors != null && item.Value.Errors.Any())
                    {
                        message.Append(string.Join(". ", item.Value.Errors.Select(x => x.ErrorMessage)));
                    }
                }

                throw new ArgumentException(message.ToString());
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}
