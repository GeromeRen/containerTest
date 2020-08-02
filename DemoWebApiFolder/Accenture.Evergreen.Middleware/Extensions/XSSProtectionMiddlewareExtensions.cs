namespace Accenture.Evergreen.Middleware.Extensions
{
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// Defines the <see cref="XSSProtectionMiddlewareExtensions" />.
    /// </summary>
    public static class XSSProtectionMiddlewareExtensions
    {
        /// <summary>
        /// The AddXssProtectionHeader.
        /// </summary>
        /// <param name="builder">The builder<see cref="IApplicationBuilder"/>.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder AddXssProtectionHeader(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ContentSinffingMiddleware>();
        }
    }
}
