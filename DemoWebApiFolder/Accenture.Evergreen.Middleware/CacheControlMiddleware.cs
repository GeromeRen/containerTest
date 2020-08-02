using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Accenture.Evergreen.Middleware
{
    public class CacheControlMiddleware
    {

        /// <summary>
        /// Defines the _next.
        /// </summary>
        private readonly RequestDelegate _next;


        /// <summary>
        /// Initializes a new instance of the <see cref="CacheControlMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next<see cref="RequestDelegate"/>.</param>
        public CacheControlMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <summary>
        /// The InvokeAsync.
        /// </summary>
        /// <param name="context">The context<see cref="HttpContext"/>.</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Response.GetTypedHeaders().CacheControl =
             new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
             {
                 Public = true,
                 NoStore = false,
                 MaxAge = TimeSpan.FromDays(365)
             };
            httpContext.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                new string[] { "Accept-Encoding" };

            await _next(httpContext);
        }
    }
}
