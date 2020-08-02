using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Accenture.Evergreen.Middleware
{
    public class ContentSinffingMiddleware
    {
        /// <summary>
        /// Defines the _next.
        /// </summary>
        private readonly RequestDelegate _next;


        /// <summary>
        /// Initializes a new instance of the <see cref="ContentSinffingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next<see cref="RequestDelegate"/>.</param>
        public ContentSinffingMiddleware(RequestDelegate next)
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
            httpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");

            await _next(httpContext);
        }
    }
}
