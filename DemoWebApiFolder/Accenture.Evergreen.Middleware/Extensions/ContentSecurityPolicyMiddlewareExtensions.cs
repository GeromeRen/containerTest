using Microsoft.AspNetCore.Builder;
using System.Diagnostics.CodeAnalysis;

namespace Accenture.Evergreen.Middleware.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ContentSecurityPolicyMiddlewareExtensions
    {
        /// <summary>
        /// Adds Content-Security-Policy to response's header.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder AddCSPHeader(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ContentSinffingMiddleware>();
        }
    }
}
