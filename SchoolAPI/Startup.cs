using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using School.Data.DataAccess.Repositories;
using School.Data.Database;
using System.Text.Json.Serialization;
using Serilog;
using System;

namespace SchoolAPI
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

            services.AddControllers(options =>
                { 
                    options.RespectBrowserAcceptHeader = true; // false by default
                })
                // Add XML formatter support
                // .AddXmlSerializerFormatters()
                .AddNewtonsoftJson(options =>
                {
                    // Setting required to include child objects in json output
                    options.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    // Use the default property (Pascal) casing
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SchoolAPI", Version = "v1" });
            });

            #region Database configuration
            //Local DB Register
            services.AddDbContext<SchoolDbContext>(option =>
                option.UseSqlServer(Configuration.GetConnectionString("DbConnection"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            );
            #endregion

            services.AddTransient<IProgrammeRepository, ProgrammeRepository>();
            services.AddTransient<IStudentRepository, StudentRepository>();

            // Assemblies scanned for profiles of mapping configurations
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SchoolAPI v1"));
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();

            app.UseHttpsRedirection();

            // All requests details will be logged
            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
