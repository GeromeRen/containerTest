using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using Serilog;

namespace MyDemoWebApi3_1_5064.WebApi
{
    public class Program
    {
        public static IConfiguration Configuration
{
    get;
}

= new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional:false, reloadOnChange:true).AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional:false, reloadOnChange:true).AddEnvironmentVariables().Build();
        public static void Main(string[] args)
        {
    try
    {
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();
        Log.Information("Starting web host");
        CreateHostBuilder(args).Build().Run();
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "Host terminated unexpectedly");
    }
    finally
    {
        Log.CloseAndFlush();
    }
}

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.UseStartup<Startup>();
    webBuilder.UseKestrel(c => c.AddServerHeader = false);
}

).UseSerilog();
    }
}
