using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PTMS.Api.Config
{
    public class NotFoundResultFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult && objectResult.Value == null)
            {
                throw new KeyNotFoundException("Элемент не найден");
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}
