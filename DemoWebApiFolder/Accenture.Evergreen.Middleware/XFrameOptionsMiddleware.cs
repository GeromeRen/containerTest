using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Accenture.Evergreen.Middleware
{
    public class XFrameOptionsMiddleware
    {
        private RequestDelegate _next;

        public XFrameOptionsMiddleware(RequestDelegate next)
        {
            this._next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <summary>
        /// The InvokeAsync.
        /// </summary>
        /// <param name="context">The context<see cref="HttpContext"/>.</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("X-Frame-Options", "DENY");

            await _next(httpContext);
        }
    }
}
