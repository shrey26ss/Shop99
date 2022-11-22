using Entities.Models;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Service.Identity;
using Services.Identity;
using System;
using System.Text;
using WebApp.Middleware;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 20, DelaysInSeconds = new int[] { 300 } });
            services.RegisterService(Configuration);
            services.AddControllersWithViews();
            services.AddMvc();
            services.AddHsts(option =>
            {
                option = new Microsoft.AspNetCore.HttpsPolicy.HstsOptions
                {
                    IncludeSubDomains = true,
                    Preload = true,
                    MaxAge = TimeSpan.FromDays(7),
                };
            });
            services.AddIdentity<ApplicationUser, ApplicationRole>();
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = false;
            });
            //#endregion
            services.Configure<JWTConfig>(Configuration.GetSection("JWT"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDataProtection().SetApplicationName($"{WebHostEnvironment.EnvironmentName}")
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo($@"{WebHostEnvironment.ContentRootPath}\keys"));
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Infrastructure.Interface.ILog logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/Status404");
                app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());
                app.UseHsts();
            }
            //app.UseStatusCodePagesWithRedirects();
            //app.ConfigureExceptionHandler(logger);
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfireDashboard("/mydashboard", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=login}/{id?}");
            });
        }
    }
}