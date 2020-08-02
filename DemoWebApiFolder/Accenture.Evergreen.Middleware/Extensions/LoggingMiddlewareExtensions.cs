﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by evergreen tool.
//     NOTE:Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Accenture.Evergreen.Middleware.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Serilog;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Defines the <see cref="LoggingMiddlewareExtensions" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class LoggingMiddlewareExtensions
    {
        /// <summary>
        /// The UseEvergreenLoggingAspect.
        /// </summary>
        /// <param name="builder">The builder<see cref="IApplicationBuilder"/>.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseEvergreenLoggingAspect(this IApplicationBuilder builder)
        {
            // set up Serilog Request Logging
            builder.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    foreach (var item in httpContext.Items)
                    {
                        diagnosticContext.Set(item.Key.ToString(), item.Value.ToString());
                    }
                };
            });
            
            // set up our supplementary logging and properties
            builder.UseMiddleware<LoggingMiddleware>();
            
            return builder;
        }
    }
}