using Microsoft.AspNetCore.Builder;
using System.Diagnostics.CodeAnalysis;

namespace Accenture.Evergreen.Middleware.Extensions
{
    public static class ApplicationSecurityStandard
    {
        /// <summary>
        /// Enables all application security standards.
        /// ContentSinffingMiddleware
        /// CacheControlMiddleware
        /// ContentSecurityPolicyMiddleware
        /// XFrameOptionsMiddleware
        /// XSSProtectionMiddleware
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public static IApplicationBuilder EnableApplicationSecurityStandards(this IApplicationBuilder builder)
        {
            return builder
                 .UseMiddleware<ContentSinffingMiddleware>()
                 .UseMiddleware<CacheControlMiddleware>()
                 .UseMiddleware<XSSProtectionMiddleware>()
                 .UseMiddleware<ContentSecurityPolicyMiddleware>()
                 .UseMiddleware<XFrameOptionsMiddleware>()
                 .UseMiddleware<OtherHTTPHeadersMiddleware>();

        }
    }
}
