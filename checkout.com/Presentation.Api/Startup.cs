using System.IO;
using System.Reflection;
using Checkout.com.Common.Configuration;
using Checkout.com.Common.Logging.Implementations;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Presentation.Api.Setup;

namespace Presentation.Api
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ApplicationSettings applicationSettings = this.configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();
            Checkout.com.Common.Logging.ILog logger = this.SetupLogging();

            services.AddMvcCore().AddApiExplorer();
            services
                .AddSingleton(applicationSettings)
                .AddSingleton<Checkout.com.Common.Logging.ILog>(logger)
                .SetupDependencies(applicationSettings)
                .AddSwaggerGen(swagger =>
                {
                    swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "Payments Gateway", Version = "v1" });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseSwagger()
                .UseSwaggerUI(swagger =>
                {
                    swagger.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                    swagger.SwaggerEndpoint("/swagger/v1/swagger.json", "Checkout.com Payments");
                });

            app.UseRouting();

            app
               .UseRouting()
               .UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private Checkout.com.Common.Logging.ILog SetupLogging()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            var logger = new Log4NetLogger();
            return logger;
        }
    }
}