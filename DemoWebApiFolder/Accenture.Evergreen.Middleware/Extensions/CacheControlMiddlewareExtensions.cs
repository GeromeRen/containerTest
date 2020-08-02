namespace Accenture.Evergreen.Middleware.Extensions
{
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// Defines the <see cref="CacheControlMiddlewareExtensions" />.
    /// </summary>
    public static class CacheControlMiddlewareExtensions
    {
        /// <summary>
        /// The AddCacheControl.
        /// </summary>
        /// <param name="builder">The builder<see cref="IApplicationBuilder"/>.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder AddCacheControl(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CacheControlMiddleware>();
        }
    }
}
