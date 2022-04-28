using CandySugar.Web.Core.Commom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CandySugar.Web
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception != null)
            {
                context.Result = new JsonResult(new Result { Code = 500, Response = context.Exception.Message })
                {
                    ContentType = "application/json",
                    StatusCode = 200
                };
            }
        }
    }
}
