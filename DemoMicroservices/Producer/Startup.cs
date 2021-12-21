using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Producer
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
            var configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json")
              .Build();

            Log.Logger = new Serilog.LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddDaprClient();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Producer", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Producer v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            // Enable Middleware for reading Cloud Events as Requests
            app.UseCloudEvents();

            app.UseEndpoints(endpoints =>
            {
                // Enable Publish-Subscribe handler (from Dapr)
                endpoints.MapSubscribeHandler();
                endpoints.MapControllers();
            });
        }
    }
}
