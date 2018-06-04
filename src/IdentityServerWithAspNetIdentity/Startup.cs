using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer.Core.Shared;
using IdentityServer.Persistence;
using IdentityServer.Core;
using IdentityServer.Persistence.EF;
using IdentityServer.Authentication;
using Microsoft.EntityFrameworkCore;

namespace IdentityServerWithAspNetIdentity
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            var authService = new AuthenticationServices(null, null, null, null, null, null);
            authService.InitializeContext(services, Configuration);
            services.AddScoped<IAuthentication, AuthenticationServices>();


            //Add persistence service
            services.AddScoped<IPersistenceContext, PersistenceContext>();
            var dataService = services.BuildServiceProvider().GetService<IPersistenceContext>();
            dataService.InitializeContext(services, Configuration);

            //Add Business Layer 
            services.AddScoped<IBusinessLayer, BusinessLogic>(s => new BusinessLogic(dataService));

            services.AddMvc()
                    .AddJsonOptions(
                                    options => options.SerializerSettings.ReferenceLoopHandling= Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;

                    options.ApiName = "evaluationFormsManager";
                });


            //services.Configure<IISOptions>(iis =>
            //{
            //    iis.AuthenticationDisplayName = "Windows";
            //    iis.AutomaticAuthentication = false;
            //});
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();


        }
    }
}
