using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Steeltoe.Connector.RabbitMQ;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.Tracing;
using WeatherWebServer.Services;

namespace WeatherWebServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRabbitMQConnection(Configuration);
            services.AddAllActuators(Configuration);
            services.ActivateActuatorEndpoints();
            services.AddDistributedTracingAspNetCore();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeatherWebServer", Version = "v1" });
            });

            services.AddHttpClient();
            services.AddSingleton<WeatherRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherWebServer"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                
                endpoints.Map<HealthEndpoint>();
            });
        }
    }
}
