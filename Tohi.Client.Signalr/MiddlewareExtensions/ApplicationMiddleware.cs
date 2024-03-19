using Microsoft.Extensions.Primitives;

namespace Tohi.Client.Signalr.MiddlewareExtensions
{
    public class ApplicationMiddleware : IMiddleware
    {
        private readonly IJwtHepler _jwtHelper;

        public ApplicationMiddleware(IJwtHepler jwtHepler)
        {
            _jwtHelper = jwtHepler;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                var authorizationHeader = context.Request.Query["access_token"];
                var accessToken = authorizationHeader == StringValues.Empty ? null : authorizationHeader.Single().Split(" ").Last();
                if (accessToken != null)
                {
                    var principal = _jwtHelper.GetPrincipalFromExpiredToken(accessToken);
                    context.User = principal;
                }
                else context.User = null;
                await next(context);
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
