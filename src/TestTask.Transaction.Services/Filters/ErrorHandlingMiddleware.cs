using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using TestTask.Transaction.Common.Exceptions;

namespace TestTask.Identity.Services.Filters
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var result = JsonConvert.SerializeObject(GetExceptionModel(exception));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)GetStatusCode(exception);
            return context.Response.WriteAsync(result);
        }

        private HttpStatusCode GetStatusCode(Exception exception)
        {
            if (exception is BusinessLogicException)
            {
                return HttpStatusCode.BadRequest; //400
            }

            if (exception is NotFoundException)
            {
                return HttpStatusCode.NotFound; //404
            }

            if (exception is UnauthorizedException)
            {
                return HttpStatusCode.Unauthorized; //403
            }

            return HttpStatusCode.InternalServerError; //500
        }

        private ExceptionModel GetExceptionModel(Exception exception)
        {
            return new ExceptionModel
            {
                Message = exception.Message
            };
        }
    }
}
