using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Logging;
using Steeltoe.Management.Tracing;
using Steeltoe.Stream.Extensions;
using WeatherWebServer.Services;

namespace WeatherWebServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseCloudHosting(25542)
                .AddStreamServices<WeatherRepository>()
                .ConfigureLogging((context, builder) => builder.AddDynamicConsole())
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
