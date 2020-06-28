using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restify3SP;
using System;
using System.Net;

namespace asw.test.restifyspsqlserver
{
    public class Startup
    {
        private string dbName;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton(typeof(DatabaseAccess), new DatabaseAccess(this.Configuration.GetConnectionString("Db")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.dbName = (string)this.Configuration.GetValue(typeof(string), "DataBaseName");
            if (String.IsNullOrWhiteSpace(dbName))
            {
                throw new Exception("Target database is not configured in appsettings. Please check configuration setting DataBaseName.");
            }

            app.UseExceptionHandler($"/{dbName}/error");

            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments($"/{dbName}"))
                {
                    await next.Invoke();
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsync($"Database specified in {context.Request.Path} not supported. Service is restricted to use datbase {dbName} only.");
                }
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{schema}/{procedurename}/{arguments}");

                endpoints.MapControllerRoute(
                    "error",
                    "error");
            });
        }
    }
}
