using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LandonApi.Filters;
using LandonApi.Models;
using LandonApi.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSwag.AspNetCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace LandonApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private readonly string _devCorsPolicy = "devPolicy";
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var dbConfig = Configuration.GetSection("Db")
                .Get<DbConfig>();

            services.AddDbContextPool<ApiContext>(
                opts => opts.UseMySql(
                    $"Server={dbConfig.Server};"
                    + $"Database={dbConfig.Database};"
                    + $"User={dbConfig.User};"
                    + $"Password={dbConfig.Password};"
                ,
                mySqlOptions => 
                {
                    mySqlOptions.ServerVersion(new Version(8, 0, 15), ServerType.MySql);
                }
            ));

            services.Configure<HotelInfo>(
                Configuration.GetSection("Info")
            );
            services.AddMvc(opts =>
            {
                opts.Filters.Add<JsonExceptionFilter>();
                opts.Filters.Add<RequireHttpsOrCloseAttribute>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            services.AddRouting(opts => opts.LowercaseUrls = true);
            services.AddApiVersioning(opts => 
            {
               opts.DefaultApiVersion = new ApiVersion(1, 0);
               opts.ApiVersionReader = new MediaTypeApiVersionReader();
               opts.AssumeDefaultVersionWhenUnspecified = true;
               opts.ReportApiVersions = true;
               opts.ApiVersionSelector = new CurrentImplementationApiVersionSelector(opts);
            });

            services.AddCors(opts => 
            {
                opts.AddPolicy(_devCorsPolicy,
                    policy => policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                );
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUi3WithApiExplorer(opts => 
                {
                    opts.GeneratorSettings.DefaultPropertyNameHandling = NJsonSchema.PropertyNameHandling.CamelCase;
                });
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors(_devCorsPolicy);
            app.UseMvc();
        }
    }
}
