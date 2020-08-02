namespace Accenture.Evergreen.Middleware.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Defines the <see cref="XFrameOptionsMiddlewareExtensions" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class XFrameOptionsMiddlewareExtensions
    {
        /// <summary>
        /// The AddXFrameOptions.
        /// </summary>
        /// <param name="builder">The builder<see cref="IApplicationBuilder"/>.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder AddXFrameOptions(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<XFrameOptionsMiddleware>();
        }
    }
}
