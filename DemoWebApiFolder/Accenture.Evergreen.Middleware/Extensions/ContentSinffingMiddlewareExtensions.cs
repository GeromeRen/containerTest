namespace Accenture.Evergreen.Middleware.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Defines the <see cref="ContentSinffingMiddlewareExtensions" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ContentSinffingMiddlewareExtensions
    {
        /// <summary>
        /// The EnableContentSniffing.
        /// </summary>
        /// <param name="builder">The builder<see cref="IApplicationBuilder"/>.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder EnableContentSniffing(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ContentSinffingMiddleware>();
        }
    }
}
