﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by evergreen tool.
//     NOTE:Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Accenture.Evergreen.Middleware
{
    using Accenture.Evergreen.Middleware.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ExceptionHandlingMiddleware" />.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        /// <summary>
        /// Defines the _next.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Defines the _logger.
        /// </summary>
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next<see cref="RequestDelegate"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{ExceptionHandlingMiddleware}"/>.</param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// The InvokeAsync.
        /// </summary>
        /// <param name="context">The context<see cref="HttpContext"/>.</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if(context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                // log exception as-is, before handling it in HandleExceptionAsync method
                _logger.LogError(ex, $"An exception has been thrown. Exception Message: {ex.Message} - Logging Exception now and returning a user-friendly message. This exception and user message is linked via this ID: {context.TraceIdentifier}");
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// The HandleExceptionAsync.
        /// </summary>
        /// <param name="context">The context<see cref="HttpContext"/>.</param>
        /// <param name="ex">The ex<see cref="Exception"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = env == Microsoft.Extensions.Hosting.Environments.Development;

            if(isDevelopment)
            {
                // return the actual exception message since it's the dev environment
                var exceptionModel = new ExceptionHandlingResponseModel()
                {
                    CorrelationId = context.TraceIdentifier,
                    Message = ex.Message,
                    StatusCode = context.Response.StatusCode,
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize<ExceptionHandlingResponseModel>(exceptionModel));
            }
            else
            {
                // mask the actual error message for all other environments
                var exceptionModel = new ExceptionHandlingResponseModel()
                {
                    CorrelationId = context.TraceIdentifier,
                    Message = $"An error has occurred. Please provide the following ID to the Support Team: {context.TraceIdentifier}",
                    StatusCode = context.Response.StatusCode,
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize<ExceptionHandlingResponseModel>(exceptionModel));
            }
        }
    }
}