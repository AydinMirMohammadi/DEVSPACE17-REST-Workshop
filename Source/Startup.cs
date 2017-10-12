using System.Buffers;
using CustomerDemo.Domain.Customer;
using CustomerDemo.Hypermedia.EntryPoint;
using CustomerDemo.Util.GloblaExceptionHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WebApiHypermediaExtensionsCore.WebApi.ExtensionMethods;

namespace CustomerDemo
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
            services.AddMvc(options =>
            {
                options.OutputFormatters.Clear();
                options.OutputFormatters.Add(new JsonOutputFormatter(
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    }, ArrayPool<char>.Shared));

                // Initializes and adds the Hypermedia Extensions
                options.AddHypermediaExtensions();
                options.Filters.Add(new GlobalExceptionFilter(null));
            });

            // Required by Hypermedia Extensions
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // Domain
            services.AddSingleton<ICustomerRepository, CustomerRepository>();

            services.AddSingleton<HypermediaEntryPoint>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
