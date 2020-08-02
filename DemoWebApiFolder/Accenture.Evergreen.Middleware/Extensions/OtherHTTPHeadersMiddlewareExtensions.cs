namespace Accenture.Evergreen.Middleware.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Defines the <see cref="OtherHTTPHeadersMiddlewareExtensions" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class OtherHTTPHeadersMiddlewareExtensions
    {
        /// <summary>
        /// Removes insecure http headers from each response such as
        /// X-Powered-By
        /// Server
        /// X-AspNetMvc-Version
        /// X-AspNet-Version
        /// </summary>
        /// <param name="builder">The builder<see cref="IApplicationBuilder"/>.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder RemoveInsecureHTTPHeaders(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OtherHTTPHeadersMiddleware>();
        }
    }
}
