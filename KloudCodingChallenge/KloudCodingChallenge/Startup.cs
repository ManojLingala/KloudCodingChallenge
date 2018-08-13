using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KloudCodingChallenge.Model.Interfaces;
using KloudCodingChallenge.Model.Entities;
using KloudCodingChallenge.Service.Interfaces;
using KloudCodingChallenge.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KloudCodingChallenge
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources. Dont need this step if we keep configure file in root level

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(env.ContentRootPath, "Configs"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddOptions();

			services.Configure<AppConfig>(Configuration);
            var serviceConfig = Configuration.GetSection("Service");
                                                    
			services.Configure<ServiceConfig>(serviceConfig);

            services.AddTransient<IDataService, DataService>();
            services.AddTransient<IOwner, Owner>();
            services.AddTransient<ICar, Car>();
            services.AddTransient<ICacheService, MemoryCacheService>();

            // Restsharp has a bug when run on MACOS so come back to use normal HTTPClient, I wish my app can fully support cross platform.
            // Reason I use single instance of HttpClient is to avoid exhausted port due to TIME_WAIT design. - But this app really small so it not an issue at all.

            var restClient = new HttpClient()
            {
                BaseAddress = new Uri(serviceConfig.Get<ServiceConfig>().RootAPIUrl)
            };

            services.AddSingleton<HttpClient>(restClient);
            services.AddMvc();
            services.AddMemoryCache();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //Uncomment to test the basic I.P whitelisting(127.0.0.1) and the API Identity Key checks
            //Check the IPAuthenticationFilter class for more details 

            //// Add framework services.  
            //services.AddMvc(options => {
            //    //By the type  
            //    options.Filters.Add(typeof(Filters.IPAuthenticationFilter));
            //});

        }
    }
}
