﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by evergreen tool.
//     NOTE:Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Accenture.Evergreen.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Serilog.Context;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="LoggingMiddleware" />.
    /// </summary>
    public class LoggingMiddleware
    {
        /// <summary>
        /// Defines the next delegate to execute.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next<see cref="RequestDelegate"/>.</param>
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Performs the logging on the given context and invoke the next delegate on success.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="HttpContext"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task Invoke(HttpContext httpContext)
        {
            // Set up values
            var requestTime = DateTime.UtcNow;
            var correlationId = httpContext.TraceIdentifier;

            // add properties to the context items collection
            // These will be added to the logging context upon request completion
            httpContext.Items.Add("RequestTime", requestTime);
            httpContext.Items.Add("CorrelationId", correlationId);

            // add properties that we wish to enrich ALL logs with
            LogContext.PushProperty("CorrelationId", correlationId);

            await _next(httpContext);
        }
    }
}
