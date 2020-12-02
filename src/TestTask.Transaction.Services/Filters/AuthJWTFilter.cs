using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask.Transaction.BL.Services;
using TestTask.Transaction.Common;
using TestTask.Transaction.Common.Exceptions;

namespace TestTask.Transaction.Services.Filters
{
    public class AuthJWTFilter : BaseAuthJWTFilter
    {
        public AuthJWTFilter(IOptions<AppSettings> appSettings, 
                            IAuthService authService)
            : base( appSettings, authService)
        { }
    }

    public class AuthJWTAttribute : TypeFilterAttribute
    {
        public AuthJWTAttribute() : base(typeof(BaseAuthJWTFilter))
        {

        }
    }

    public class BaseAuthJWTFilter : IAuthorizationFilter
    {
        protected readonly AppSettings appSettings;
        protected readonly IAuthService authService;

        public BaseAuthJWTFilter(IOptions<AppSettings> appSettings, 
                                  IAuthService authService)
        {
            this.appSettings = appSettings.Value;
            this.authService = authService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers.SingleOrDefault(x => x.Key.ToLower() == "resttoken");
            if (token.Equals(default(KeyValuePair<string, StringValues>)))
            {
                throw new UnauthorizedException("User is not signed in.");
            }

            Task.WaitAll(authService.ValidateToken(token.Value));
        }
    }
}
