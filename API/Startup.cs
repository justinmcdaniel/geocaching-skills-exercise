using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Data.Interfaces;
using FluentValidation.AspNetCore;
using Newtonsoft.Json;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<GeocacheDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("GeocacheContext")));

            services.AddScoped<IGeocacheRepository, EFGeocacheRepository>();
            services.AddScoped<IItemRepository, EFItemRepository>();

            services.AddControllers()
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddNewtonsoftJson(options => {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddSwaggerGen(c =>
            {
                var apiVersion = Configuration.GetValue<string>("APIVersion");
                c.SwaggerDoc(apiVersion, new OpenApiInfo { Title = "Geocache API", Version = apiVersion });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    var apiVersion = Configuration.GetValue<string>("APIVersion");
                    c.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json", $"Geocache {apiVersion}");
                });
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
