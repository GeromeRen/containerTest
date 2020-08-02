using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Accenture.Evergreen.Middleware.Extensions;
using Microsoft.Identity.Web;

namespace MyDemoWebApi3_1_5064.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.EnforceHTTPS();
            services.AddAuthentication();
            services.AddProtectedWebApi(Configuration);
            services.AddAuthValidationMiddleware(Configuration);
            services.AddHealthChecks();
            services.AddOptions();
            services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins, builder =>
    {
        builder.WithOrigins("http://example.com", "http://www.contoso.com");
    }

    );
}

);
            services.AddEvergreenCorrelationIdMiddlewareOptions(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseEvergreenMSALAspect();

            app.UseHsts();

            app.UseHttpsRedirection();

            app.EnableApplicationSecurityStandards();

            app.UseEvergreenCorrelationIdAspect();

            app.UseEvergreenLoggingAspect();

            app.UseEvergreenExceptionAspect();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
