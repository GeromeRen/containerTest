using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Accenture.Evergreen.Middleware
{
    public class ContentSecurityPolicyMiddleware
    {
        /// <summary>
        /// Defines the _next.
        /// </summary>
        private readonly RequestDelegate _next;

        public ContentSecurityPolicyMiddleware(RequestDelegate next)
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
            httpContext.Response.Headers.Add(
            "Content-Security-Policy",
            "default-src 'self' 'unsafe-eval' 'unsafe-inline' *.accenture.com; " +
            "img-src 'self' *.accenture.com data:; connect-src 'self' *.accenture.com;  " +
            "font-src 'self' *.accenture.com report-uri *.accenture.com/csp_report; " +
            "connect-src 'self' *.accenture.com; " +
            "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
            "form-action 'self';");

            await _next(httpContext);
        }
    }
}
