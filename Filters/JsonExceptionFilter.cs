using LandonApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LandonApi.Filters
{
    public class JsonExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _env;
        public JsonExceptionFilter(IHostingEnvironment env)
        {
            _env = env;
        }
        public void OnException(ExceptionContext ctx)
        {
            var error = new ApiError();

            if (_env.IsDevelopment())
            {
                error.Message = ctx.Exception.Message;
                error.Detail = ctx.Exception.StackTrace;
            }
            else
            {
                error.Message = "There was an error processing your request";
                error.Detail = ctx.Exception.Message;
            }

            ctx.Result = new ObjectResult(error)
            {
                StatusCode = 500
            };
        }
    }
}
