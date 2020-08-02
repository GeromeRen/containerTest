using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Accenture.Evergreen.Middleware.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class HSTSMiddlewareExtensions
    {
      
        public static IServiceCollection EnforceHTTPS(this IServiceCollection services)
        {
            return services.AddHsts(options =>
            {
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });
        }
    }
}
