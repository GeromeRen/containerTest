namespace Accenture.Evergreen.Middleware
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="OtherHTTPHeadersMiddleware" />.
    /// </summary>
    public class OtherHTTPHeadersMiddleware
    {
        /// <summary>
        /// Defines the _next.
        /// </summary>
        private RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="OtherHTTPHeadersMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next<see cref="RequestDelegate"/>.</param>
        public OtherHTTPHeadersMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <summary>
        /// The InvokeAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="HttpContext"/>.</param>
        /// <returns>.</returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            if(httpContext.Response.Headers.Any(c => c.Key.ToLower() == "x-powered-by"))
            {
                httpContext.Response.Headers.Remove("x-powered-by");
            }
            if(httpContext.Response.Headers.Any(c => c.Key.ToLower() == "server"))
            {
                httpContext.Response.Headers.Remove("server");
            }
            if(httpContext.Response.Headers.Any(c => c.Key.ToLower() == "x-aspnet-version"))
            {
                httpContext.Response.Headers.Remove("X-AspNet-Version");
            }
            if(httpContext.Response.Headers.Any(c => c.Key.ToLower() == "x-aspnetmvc-version"))
            {
                httpContext.Response.Headers.Remove("X-AspNetMvc-Version");
            }

            await _next(httpContext);
        }
    }
}
