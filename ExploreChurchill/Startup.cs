using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExploreChurchill
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
            services.AddMvc(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory
            )
        {



            //redirect to error page incase of error

            app.UseExceptionHandler("/error.html");
            app.UseRouting();
            // dev stack traces

            if (env.IsDevelopment())
            
            // Or using the custom config

            // if (configuration["EnableDeveloperExceptions"] == "True")

            {
                app.UseDeveloperExceptionPage();
            }

        

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.Contains("invalid"))
                    throw new Exception("Error!");

                await next();
            });


            // Call the dotnet MVC
            // Note how the controller maps to 'homecontroller' in the controllers folder. The id param only matches an int
            // using those matching params good for security 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("Default",
                    "{controller=Home}/{action=Index}/{id:int?}"
                    );
            });


            // example for serve static files from wwwroot

            app.UseFileServer();
        }
    }
}
